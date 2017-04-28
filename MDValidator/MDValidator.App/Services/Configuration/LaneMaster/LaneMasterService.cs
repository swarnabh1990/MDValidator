using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MDValidator.App.Areas.Configuration.Models.LaneMaster;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MDValidator.App.Utilities;
using MDValidator.Domain.Configuration.LaneMaster;

namespace MDValidator.App.Services.Configuration.LaneMaster
{
    public class LaneMasterService : ILaneMasterService
    {
        public LaneMasterViewModel ProcessFile(string fileName)
        {
            LaneMasterViewModel vm = new LaneMasterViewModel();

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                #region VALIDATE FILE TEMPLATE

                string requiredTemplate = "LANE ORIGIN DESTINATION MTART LEG SITE SYSID SCENARIO HERKL BUKRS WERKS VKORG VTWEG SPART SLOC EKORG EKGRP INFNR KSCHL WAERS  BSTAE DOCTYPE KUNWE KUNRE INCO1 KUNAG KUNRG LIFNR LIFN2 KTGRD LZONE BWART ZTERM";
                int totalColumns = 34;

                bool validTemplate = OpenXML_SpreadSheet.ValidateTemplate(requiredTemplate, sheetData.Elements<Row>().First(), workbookPart, vm.Errors);

                if (!validTemplate)
                {
                    return vm;
                }

                #endregion

                #region READ FILE DATA

                List<List<string>> rows = OpenXML_SpreadSheet.ReadFile(sheetData, workbookPart, totalColumns, vm.Errors);

                DateTime date = DateTime.Now;

                int rowNum = 2;
                
                //load View Model from file Data
                foreach (var row in rows)
                {
                    LaneMasterRecord record = new LaneMasterRecord()
                    {
                        Lane = row[0],
                        Origin = row[1],
                        Destination = row[2],
                        MTART = row[3],
                        Leg = OpenXML_SpreadSheet.GetIntValue(row[4]),
                        Site = row[5],
                        SYSID = row[6],
                        Scenario = row[7],
                        HERKL = row[8],
                        BUKRS = row[9],
                        WERKS = row[10],
                        VKORG = row[11],
                        VTWEG = row[12],
                        SPART = row[13],
                        SLOC = row[14],
                        EKORG = row[15],
                        EKGRP = row[16],
                        INFNR = row[17],
                        KSCHL = row[18],
                        WAERS = row[19],
                        MasterConditionRecordForPO = row[20],
                        BSTAE = row[21],
                        DOCTYPE = row[22],
                        KUNWE = row[23],
                        KUNRE = row[24],
                        INCO1 = row[25],
                        KUNAG = row[26],
                        KUNRG = row[27],
                        LIFNR = row[28],
                        LIFN2 = row[29],
                        KTGRD = row[30],
                        LZONE = row[31],
                        BWART = row[32],
                        ZTERM = row[33],
                        DateTime = date,
                        User = "#####"
                    };

                    //check for completeness
                    if (string.IsNullOrEmpty(record.Lane) || string.IsNullOrWhiteSpace(record.Lane) ||
                        string.IsNullOrEmpty(record.Origin) || string.IsNullOrWhiteSpace(record.Origin) ||
                        string.IsNullOrEmpty(record.Destination) || string.IsNullOrWhiteSpace(record.Destination) ||
                        string.IsNullOrEmpty(record.MTART) || string.IsNullOrWhiteSpace(record.MTART) ||
                        record.Leg < 1 ||
                        string.IsNullOrEmpty(record.Site) || string.IsNullOrWhiteSpace(record.Site) ||
                        string.IsNullOrEmpty(record.SYSID) || string.IsNullOrWhiteSpace(record.SYSID))
                    {
                        vm.Errors.Add("Missing data in line " + rowNum);
                    }

                    vm.LaneMaster.Add(record);
                    rowNum++;
                }

                //this.ReadFile(sheetData, workbookPart, vm.OfficialList, vm.Errors);
                #endregion
            
            };

            return vm;
        }
    }
}
