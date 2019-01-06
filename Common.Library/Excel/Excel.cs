using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;

namespace Common.Excel
{
    public class Excel
    {
        public FileInfo File { get; set; }
        public ExcelPackage Package { get; set; }
        public ExcelWorkbook Book { get; set; }
        public ExcelWorksheets Worksheets { get; set; }
        public int SheetsCount { get; set; }
        public List<string> SheetsName { get; set; }

        public Excel(string filename)
        {
            this.File = new FileInfo(filename);
            this.Package = new ExcelPackage(this.File);
            this.Book = this.Package.Workbook;
            this.Worksheets = this.Book.Worksheets;
            this.SheetsCount = this.Worksheets.Count;
            if (this.SheetsName == null)
            {
                this.SheetsName = new List<string>();
            }
            foreach (var sheet in this.Worksheets)
            {
                this.SheetsName.Add(sheet.Name);
            }
        }

        public Excel(FileInfo file)
        {
            this.File = file;
            this.Package = new ExcelPackage(this.File);
            this.Book = this.Package.Workbook;
            this.Worksheets = this.Book.Worksheets;
            this.SheetsCount = this.Worksheets.Count;
            foreach (var sheet in this.Worksheets)
            {
                this.SheetsName.Add(sheet.Name);
            }
        }
    }
}