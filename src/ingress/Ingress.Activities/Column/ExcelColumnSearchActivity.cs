using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Blob.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GoodToCode.Analytics.Ingress.Activities
{
    public class ExcelColumnSearchActivity
    {
        private readonly IExcelService service;

        public ExcelColumnSearchActivity(IExcelService serviceExcel)
        {
            service = serviceExcel;
        }

        public IEnumerable<ICellData> Execute(Stream excelStream, string documentName, string searchString)
        {
            var returnCells = new List<ICellData>();
            
            documentName = string.IsNullOrWhiteSpace(documentName) ? $"Analytics-{DateTime.UtcNow:u}" : documentName;
            var wb = service.GetWorkbook(excelStream);
            if (!wb.Sheets.Any()) throw new ArgumentException("Passed workbook/file does not have any sheets.");
            foreach (var item in wb.Sheets)
            {
                var sd = service.GetSheet(excelStream, item.SheetIndex);
                if (!sd.Rows.Any()) throw new ArgumentException("Passed sheet does not have any rows.");
                var header = sd.GetRow(1);
                var foundCells = header.Cells.Where(c => c.ColumnName.Contains(searchString));
                foreach(var cell in foundCells)
                {
                    var newCells = sd.GetColumn(cell.ColumnIndex);
                    returnCells.AddRange(newCells);
                }
            }

            return returnCells;
        }
    }
}
