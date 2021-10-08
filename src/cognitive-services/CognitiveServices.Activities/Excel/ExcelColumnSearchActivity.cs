using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Blob.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GoodToCode.Analytics.CognitiveServices.Activities
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
            var wb = service.GetWorkbook(excelStream, documentName);
            foreach (var sheet in wb.Sheets)
            {
                if (!sheet.Rows.Any()) throw new ArgumentException("Passed sheet does not have any rows.");
                var header = sheet.GetRow(1);
                var foundCells = header.Cells.Where(c => c.ColumnName.Contains(searchString));
                foreach(var cell in foundCells)
                {
                    var newCells = sheet.GetColumn(cell.ColumnIndex);
                    returnCells.AddRange(newCells);
                }
            }

            return returnCells;
        }
    }
}
