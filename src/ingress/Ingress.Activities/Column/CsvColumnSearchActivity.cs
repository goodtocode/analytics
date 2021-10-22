using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Blob.Csv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GoodToCode.Analytics.Ingress.Activities
{
    public class CsvColumnSearchActivity
    {
        private readonly ICsvService service;

        public CsvColumnSearchActivity(ICsvService serviceCsv)
        {
            service = serviceCsv;
        }

        public IEnumerable<ICellData> Execute(Stream CsvStream, string documentName, string searchString)
        {
            var returnCells = new List<ICellData>();
            
            documentName = string.IsNullOrWhiteSpace(documentName) ? $"Analytics-{DateTime.UtcNow:u}" : documentName;
            var sheet = service.GetSheet(CsvStream);
            if (!sheet.Rows.Any()) throw new ArgumentException("Passed sheet does not have any rows.");
            var header = sheet.GetRow(1);
            var foundCells = header.Cells.Where(c => c.ColumnName.Contains(searchString));
            foreach(var cell in foundCells)
            {
                var newCells = sheet.GetColumn(cell.ColumnIndex);
                returnCells.AddRange(newCells);
            }

            return returnCells;
        }
    }
}
