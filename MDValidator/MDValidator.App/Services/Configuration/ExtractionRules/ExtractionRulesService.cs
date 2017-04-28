using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MDValidator.App.Areas.Configuration.Models.ExtractionRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MDValidator.App.Utilities;
using DomainExtractionRules = MDValidator.Domain.Configuration.ExtractionRules;

namespace MDValidator.App.Services.Configuration.ExtractionRules
{
    public class ExtractionRulesService : RulesBase, IExtractionRulesService
    {
        public HeadersViewModel ProcessHeaderFile(string fileName)
        {
            HeadersViewModel vm = new HeadersViewModel();

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                #region VALIDATE FILE TEMPLATE

                string requiredTemplate = "EXTRID TABLE HIERARCHY";
                int totalColumns = 11;

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
                    DomainExtractionRules.Header header = new DomainExtractionRules.Header()
                    {
                        ExtrId = row[0],
                        Table = row[1],
                        Hierarchy = OpenXML_SpreadSheet.GetIntValue(row[2]),
                        DateTime = date,
                        User = "#####"
                    };

                    //check for completeness
                    if (string.IsNullOrEmpty(header.ExtrId) || string.IsNullOrWhiteSpace(header.ExtrId) ||
                        string.IsNullOrEmpty(header.Table) || string.IsNullOrWhiteSpace(header.Table) ||
                        header.Hierarchy < 0)
                    {
                        vm.Errors.Add("Missing data in line " + rowNum);
                    }

                    vm.Headers.Add(header);
                    rowNum++;
                }

                //this.ReadFile(sheetData, workbookPart, vm.OfficialList, vm.Errors);
                #endregion

            };

            return vm;
        }

        public ItemsViewModel ProcessItemsFile(string fileName)
        {
            ItemsViewModel vm = new ItemsViewModel();

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                #region VALIDATE FILE TEMPLATE

                string requiredTemplate = "EXTRID STEP CONTROL TABLE FIELD COMMAND PARAM_TABLE PARAM_FIELD PARAMETER CONNECTOR";
                int totalColumns = 11;

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

                    DomainExtractionRules.Item item = new DomainExtractionRules.Item()
                    {
                        ExtrId = row[0],
                        Step = OpenXML_SpreadSheet.GetIntValue(row[1]),
                        Control = row[2],
                        Table = row[3],
                        Field = row[4],
                        Command = row[5],
                        ParamTable = row[6],
                        ParamField = row[7],
                        Parameter = row[8],
                        Connector = row[9],
                        DateTime = date,
                        User = "#####"
                    };

                    //check for completeness
                    if (string.IsNullOrEmpty(item.ExtrId) || string.IsNullOrWhiteSpace(item.ExtrId) ||
                        item.Step < 1 ||
                        string.IsNullOrEmpty(item.Control) || string.IsNullOrWhiteSpace(item.Control) ||
                        string.IsNullOrEmpty(item.Table) || string.IsNullOrWhiteSpace(item.Table) ||
                        string.IsNullOrEmpty(item.ParamTable + item.ParamField + item.Parameter) || string.IsNullOrWhiteSpace(item.ParamTable + item.ParamField + item.Parameter))
                    {
                        vm.Errors.Add("Missing data in line " + rowNum);
                    }

                    vm.Items.Add(item);
                    rowNum++;
                }

                //this.ReadFile(sheetData, workbookPart, vm.OfficialList, vm.Errors);
                #endregion

            };

            return vm;
        }

        
    }
}
