using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using Sheets = DocumentFormat.OpenXml.Spreadsheet.Sheets;
using Element = DocumentFormat.OpenXml.OpenXmlElement;
using Attribute = DocumentFormat.OpenXml.OpenXmlAttribute;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using MDValidator.Domain.Validation;
using MDValidator.App.Areas.Validation.Models.Validation;
using MDValidator.App.Utilities;
using MDValidator.Domain.Configuration.LaneMaster;

namespace MDValidator.App.Services.Validation
{
    public class OfficialListService : IOfficialListService
    {
        public OfficialListViewModel ProcessFile(string fileName, List<LaneMasterRecord> laneMaster)
        {
            OfficialListViewModel vm = new OfficialListViewModel();

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                int totalColumns = 5;

                #region VALIDATE FILE TEMPLATE

                string requiredTemplate = "LANE ORIGIN DESTINATION MTART MATNR";

                bool validTemplate = OpenXML_SpreadSheet.ValidateTemplate(requiredTemplate, sheetData.Elements<Row>().First(), workbookPart, vm.Errors);

                if (!validTemplate)
                {
                    return vm;
                }

                #endregion

                //check if LANE MASTER Exists
                if (laneMaster == null || laneMaster.Count == 0)
                {
                    vm.Errors.Add("Lane Master not found!!");
                    return vm;
                }

                #region READ FILE DATA

                List<List<string>> rows = OpenXML_SpreadSheet.ReadFile(sheetData, workbookPart, totalColumns, vm.Errors);

                int rowNum = 2;
                //load View Model from file Data
                foreach (var row in rows)
                {
                    OfficialListEntry entry = new OfficialListEntry()
                    {
                        Lane = row[0],
                        Origin = row[1],
                        Destination = row[2],
                        MTART = row[3],
                        MATNR = row[4]
                    };

                    //check completeness
                    if (!entry.SatisfiesCompleteness)
                    {
                        vm.Errors.Add("Missing data in line " + rowNum);
                    }

                    //check duplicates
                    if (vm.OfficialList.Entries.Exists(of => of.Equals(entry)))
                    {
                        vm.Errors.Add("Duplicate Lane (Origin/Destination)/Material Number combination: line " + rowNum);
                    }

                    //check if exists in lane master
                    if (!laneMaster.Exists(l => l.Lane.Equals(entry.Lane) && l.Origin.Equals(entry.Origin) && l.Destination.Equals(entry.Destination) &&
                        l.MTART.Equals(entry.MTART)))
                    {
                        vm.Errors.Add("Missing Lane (Origin/Destination)/Material Type combination in Lane Master File: line " + rowNum);
                    }

                    vm.OfficialList.Entries.Add(entry);
                    rowNum++;
                }

                //this.ReadFile(sheetData, workbookPart, vm.OfficialList, vm.Errors);
                #endregion
            };

            //assign name
            vm.OfficialList.Name = "OL_" + DateTime.Now.ToString("yyyymmdd_hhmmss");

            return vm;
        }
    }
}
