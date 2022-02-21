
using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Persistence.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace GoodToCode.Analytics.Matching.Tests
{
    public class DataSourceEntity : IEntity
    {
        private string _partitionKey;
        [JsonInclude]
        public string PartitionKey { get { return _partitionKey; } set { _partitionKey = value; } }
        [JsonInclude]
        public string RowKey { get; set; } = Guid.NewGuid().ToString();
        [JsonInclude]
        public DateTimeOffset? Timestamp { get; set; } = DateTime.UtcNow;
        [JsonInclude]
        public string Address { get; set; }
        [JsonInclude]
        public string ContentType { get; set; }
        [JsonInclude]
        public string StatusCode { get; set; }
        [JsonInclude]
        public string Status { get; set; }
        [JsonInclude]
        public string Indexability { get; set; }
        [JsonInclude]
        public string IndexabilityStatus { get; set; }
        [JsonInclude]
        public string Title1 { get; set; }
        [JsonInclude]
        public string H1_1 { get; set; }
        [JsonInclude]
        public string H1_2 { get; set; }
        [JsonInclude]
        public string H2_1 { get; set; }
        [JsonInclude]
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

        public override string ToString()
        {
            return Address;
        }
    }

    public static class DataSourceEntityExtensions
    {
        public static IEnumerable<DataSourceEntity> ToDataSourceEntity(this ISheetData sheet)
        {
            IEnumerable<DataSourceEntity> returnData;

            if (!sheet.Rows.Any())
                throw new ArgumentException("Argument list is empty.", sheet.Rows.GetType().Name);

            returnData = sheet.Rows.Select(r => new DataSourceEntity(r)).ToList();

            return returnData;
        }
    }
}