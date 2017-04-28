using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MDValidator.App.Areas.Configuration.Models.ValidationRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MDValidator.App.Utilities;
using DomainValidationRules = MDValidator.Domain.Configuration.ValidationRules;

namespace MDValidator.App.Services.Configuration.ValidationRules
{
    public class ValidationRulesService : RulesBase, IValidationRulesService
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

                string requiredTemplate = "VALID VALTXT READID EXTRID ROUTINE ORIGIN DESTINATION MTART LEG SITE SYSID";
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
                    DomainValidationRules.Header header = new DomainValidationRules.Header()
                    {
                        ValId = row[0],
                        ValTxt = row[1],
                        ReadId = row[2],
                        ExtrID = row[3],
                        Routine = row[4],
                        Origin = row[5],
                        Destination = row[6],
                        MTART = row[7],
                        Leg = row[8],
                        Site = row[9],
                        SysId = row[10],
                        DateTime = date,
                        User = "#####"
                    };

                    //check for completeness
                    if (string.IsNullOrEmpty(header.ValId) || string.IsNullOrWhiteSpace(header.ValId) ||
                        string.IsNullOrEmpty(header.ValTxt) || string.IsNullOrWhiteSpace(header.ValTxt) ||
                        string.IsNullOrEmpty(header.ReadId) || string.IsNullOrWhiteSpace(header.ReadId) ||
                        string.IsNullOrEmpty(header.ExtrID) || string.IsNullOrWhiteSpace(header.ExtrID) ||
                        string.IsNullOrEmpty(header.Origin) || string.IsNullOrWhiteSpace(header.Origin) ||
                        string.IsNullOrEmpty(header.Destination) || string.IsNullOrWhiteSpace(header.Destination) ||
                        string.IsNullOrEmpty(header.MTART) || string.IsNullOrWhiteSpace(header.MTART) ||
                        string.IsNullOrEmpty(header.Leg) || string.IsNullOrWhiteSpace(header.Leg) ||
                        string.IsNullOrEmpty(header.Site) || string.IsNullOrWhiteSpace(header.Site) ||
                        string.IsNullOrEmpty(header.SysId) || string.IsNullOrWhiteSpace(header.SysId))
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

                string requiredTemplate = "VALID STEP TYPE TABLE FIELD COMMAND PARAM_TABLE PARAM_FIELD PARAMETER CONNECTOR PREREQ";
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

                    DomainValidationRules.Item item = new DomainValidationRules.Item()
                    {
                        ValId = row[0],
                        Step = OpenXML_SpreadSheet.GetIntValue(row[1]),
                        Type = row[2],
                        Table = row[3],
                        Field = row[4],
                        Command = row[5],
                        ParamTable = row[6],
                        ParamField = row[7],
                        Parameter = row[8],
                        Connector = row[9],
                        PreReq = OpenXML_SpreadSheet.GetIntValue(row[10]),
                        DateTime = date,
                        User = "#####"
                    };

                    //check for completeness
                    if (string.IsNullOrEmpty(item.ValId) || string.IsNullOrWhiteSpace(item.ValId) ||
                        item.Step < 1 ||
                        string.IsNullOrEmpty(item.Type) || string.IsNullOrWhiteSpace(item.Type) ||
                        string.IsNullOrEmpty(item.Table) || string.IsNullOrWhiteSpace(item.Table) ||
                        string.IsNullOrEmpty(item.Field) || string.IsNullOrWhiteSpace(item.Field) ||                        
                        string.IsNullOrEmpty(item.Command) || string.IsNullOrWhiteSpace(item.Command) ||
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
