using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;

namespace MDValidator.App.Utilities
{
    public class OpenXML_SpreadSheet
    {
        ///<summary>returns an empty cell when a blank cell is encountered
        ///</summary>
        public static IEnumerable<Cell> GetRowCells(Row row)
        {
            int currentCount = 0;

            foreach (DocumentFormat.OpenXml.Spreadsheet.Cell cell in
                row.Descendants<DocumentFormat.OpenXml.Spreadsheet.Cell>())
            {
                string columnName = GetColumnName(cell.CellReference);

                int currentColumnIndex = ConvertColumnNameToNumber(columnName);

                for (; currentCount < currentColumnIndex; currentCount++)
                {
                    yield return new DocumentFormat.OpenXml.Spreadsheet.Cell();
                }

                yield return cell;
                currentCount++;
            }
        }

        /// <summary>
        /// Given a cell name, parses the specified cell to get the column name.
        /// </summary>
        /// <param name="cellReference">Address of the cell (ie. B2)</param>
        /// <returns>Column Name (ie. B)</returns>
        public static string GetColumnName(string cellReference)
        {
            // Match the column name portion of the cell name.
            var regex = new System.Text.RegularExpressions.Regex("[A-Za-z]+");
            var match = regex.Match(cellReference);

            return match.Value;
        }

        /// <summary>
        /// Given just the column name (no row index),
        /// it will return the zero based column index.
        /// </summary>
        /// <param name="columnName">Column Name (ie. A or AB)</param>
        /// <returns>Zero based index if the conversion was successful</returns>
        /// <exception cref="ArgumentException">thrown if the given string
        /// contains characters other than uppercase letters</exception>
        public static int ConvertColumnNameToNumber(string columnName)
        {
            var alpha = new System.Text.RegularExpressions.Regex("^[A-Z]+$");
            if (!alpha.IsMatch(columnName)) throw new ArgumentException();

            char[] colLetters = columnName.ToCharArray();
            Array.Reverse(colLetters);

            int convertedValue = 0;
            for (int i = 0; i < colLetters.Length; i++)
            {
                char letter = colLetters[i];
                int current = i == 0 ? letter - 65 : letter - 64; // ASCII 'A' = 65
                convertedValue += current * (int)Math.Pow(26, i);
            }

            return convertedValue;
        }

        public static string GetCellText(Cell cell, WorkbookPart workbookPart)
        {
            string value = cell.InnerText; ;

            if (cell.DataType != null)
            {
                switch (cell.DataType.Value)
                {
                    case CellValues.SharedString:
                        var stringTable = workbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                        if (stringTable != null)
                        {
                            value = stringTable.SharedStringTable.
                            ElementAt(int.Parse(value)).InnerText;
                        }
                        break;
                    case CellValues.Boolean:
                        switch (value)
                        {
                            case "0":
                                value = "FALSE";
                                break;
                            default:
                                value = "TRUE";
                                break;
                        }
                        break;
                }
            }

            return value;
        }

        public static bool ValidateTemplate(string requiredTemplate, Row headerRow, WorkbookPart workbookPart, List<string> errors)
        {
            List<string> fileTemplate = new List<string>();
            foreach (Cell cell in headerRow.Elements<Cell>())
            {
                if (cell == null)
                {
                    errors.Add("Wrong file format!");
                    return false;
                }

                fileTemplate.Add(OpenXML_SpreadSheet.GetCellText(cell, workbookPart).Trim());
            }

            if (!requiredTemplate.Equals(string.Join(" ", fileTemplate.ToArray())))
            {
                errors.Add("Wrong file format!");
                return false;
            }

            return true;
        }

		public static List<List<string>> ReadFile(SheetData sheetData, WorkbookPart workbookPart, int totalColumns, List<string> errors)
        {
            List<List<string>> rows = new List<List<string>>();

            int rowNum = 1;

            foreach (Row row in sheetData.Elements<Row>())
            {                
                if (rowNum > 1)
                {
                    List<string> rowData = new List<string>();

                    IEnumerable<Cell> cells = GetRowCells(row);

                    foreach (Cell cell in cells)
                    {
                        if (cell != null)
                        {
                            rowData.Add(OpenXML_SpreadSheet.GetCellText(cell, workbookPart));
                        }
                    }

                    rows.Add(rowData);

                    if (row.Count() < totalColumns)
                    {
                        int limit = totalColumns - rowData.Count();
                        for (int i = 0; i < limit; i++)
                        {
                            rowData.Add("");
                        }
                    }
                }

                rowNum++;
            }

            return rows;
        }

        public static int GetIntValue(string value)
        {
            int result = -1;
            int.TryParse(value, out result);
            return result;
        }
    }
}
