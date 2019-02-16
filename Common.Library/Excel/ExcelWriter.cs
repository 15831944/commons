using OfficeOpenXml;

namespace System.Excel
{
    public class ExcelWriter
    {
        private Excel excel;

        private Sheet sheet;

        private ExcelWorksheet worksheet;

        public ExcelWriter(Sheet sheet)
        {
            this.sheet = sheet;
            this.excel = sheet.Excel;
            this.worksheet = sheet.Worksheet;
        }
    }
}