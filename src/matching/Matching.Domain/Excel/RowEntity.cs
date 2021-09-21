using GoodToCode.Shared.Blob.Abstractions;
using System;
using System.Text.Json.Serialization;

namespace GoodToCode.Matching.Domain
{
    public class RowEntity : IRowEntity
    {
        [JsonInclude]
        public string PartitionKey { get; private set; }
        [JsonInclude]
        public string RowKey { get; private set; }
        [JsonInclude]
        public string SheetName { get; private set; }
        [JsonInclude]
        public string ColumnName { get; private set; }
        [JsonInclude]
        public int RowIndex { get; private set; }
        [JsonInclude]
        public string CellValue { get; private set; }

        public RowEntity() { }

        public RowEntity(string rowKey, ICellData cell)
        {
            RowKey = rowKey;
            PartitionKey = cell.SheetName;
            CellValue = cell.CellValue;
            SheetName = cell.SheetName;
            ColumnName = cell.ColumnName;
            RowIndex = cell.RowIndex;
        }

        public RowEntity(ICellData cell) : this(Guid.NewGuid().ToString(), cell)
        {
        }
    }
}


