using GoodToCode.Shared.Blob.Abstractions;
using System;
using System.Text.Json.Serialization;

namespace GoodToCode.Analytics.Ingress.Domain
{
    public class RowEntity :  IRowEntity
    {
        [JsonInclude]
        public string PartitionKey { get; private set; }
        [JsonInclude]
        public string RowKey { get; private set; }
        public string WorkbookName { get; private set; }
        public int SheetIndex { get; private set; }
        [JsonInclude]
        public string SheetName { get; private set; }
        public int ColumnIndex { get; private set; }
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
            WorkbookName = cell.WorkbookName;
            SheetIndex = cell.SheetIndex;
            SheetName = cell.SheetName;
            ColumnIndex = cell.ColumnIndex;
            ColumnName = cell.ColumnName;
            RowIndex = cell.RowIndex;
            CellValue = cell.CellValue;
        }

        public RowEntity(ICellData cell) : this(Guid.NewGuid().ToString(), cell)
        {
        }
    }
}


