using OfficeOpenXml;

namespace Common.Excel
{
    public class ExcelReader
    {
        private Excel excel;
        private Sheet sheet;
        private ExcelWorksheet worksheet;

        public ExcelReader(Sheet sheet)
        {
            this.sheet = sheet;
            this.excel = sheet.Excel;
            this.worksheet = sheet.Worksheet;
        }

        public T GetCellValue<T>(int row, int col)
        {
            return this.worksheet.GetValue<T>(row, col);
        }
    }
}