using OfficeOpenXml;

namespace Common.Excel
{
    public class Sheet
    {
        public Excel Excel { get; set; }
        public ExcelWorksheet Worksheet { get; set; }
        public int RowsCount { get; set; }
        public int ColumnsCount { get; set; }

        public Sheet(Excel excel, string name)
        {
            this.Excel = excel;
            foreach (var sheet in excel.Worksheets)
            {
                if (sheet.Name == name)
                {
                    this.Worksheet = sheet;
                    this.RowsCount = sheet.Dimension.End.Row;
                    this.ColumnsCount = sheet.Dimension.End.Column;
                }
            }
        }
    }
}