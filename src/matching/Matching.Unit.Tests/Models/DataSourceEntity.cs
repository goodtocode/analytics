
using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Persistence.Abstractions;
using System;
using System.Linq;

namespace GoodToCode.Analytics.Matching.Unit.Tests
{
    public class DataSourceEntity : IEntity
    {
        private string _partitionKey;
        public string PartitionKey { get { return _partitionKey; } set { _partitionKey = value; } }
        public string RowKey { get; set; } = $"{Guid.NewGuid()}";
        public string Address { get; set; }
        public string ContentType { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public string Indexability { get; set; }
        public string IndexabilityStatus { get; set; }
        public string Title1 { get; set; }
        public string H1_1 { get; set; }
        public string H1_2 { get; set; }
        public string H2_1 { get; set; }
        public string H2_2 { get; set; }

        public DataSourceEntity() { }
        public DataSourceEntity(string partition)
        {
            _partitionKey = partition;
        }

        public DataSourceEntity(IRowData row)
        {
            var cells = row.Cells;
            if (!cells.Any())
                throw new ArgumentException("Argument list is empty.", cells.GetType().Name);

            PartitionKey = cells.FirstOrDefault().SheetName;
            RowKey = Guid.NewGuid().ToString();
            Address = cells.FirstOrDefault(c => c.ColumnName == "Address")?.CellValue;
            ContentType = cells.FirstOrDefault(c => c.ColumnName == "Content Type")?.CellValue;
            StatusCode = cells.FirstOrDefault(c => c.ColumnName == "Status Code")?.CellValue;
            Status = cells.FirstOrDefault(c => c.ColumnName == "Status")?.CellValue;
            Indexability = cells.FirstOrDefault(c => c.ColumnName == "Indexability")?.CellValue;
            IndexabilityStatus = cells.FirstOrDefault(c => c.ColumnName == "Indexability Status")?.CellValue;
            Title1 = cells.FirstOrDefault(c => c.ColumnName == "Title 1")?.CellValue;
            H1_1 = cells.FirstOrDefault(c => c.ColumnName == "H1-1")?.CellValue;
            H1_2 = cells.FirstOrDefault(c => c.ColumnName == "H1-2")?.CellValue;
            H2_1 = cells.FirstOrDefault(c => c.ColumnName == "H2-1")?.CellValue;
            H2_2 = cells.FirstOrDefault(c => c.ColumnName == "H2-2")?.CellValue;
        }
    }
}