using GoodToCode.Shared.Blob.Abstractions;
using System;

namespace GoodToCode.Analytics.Domain
{
    public class RowEntity : IRowEntity
    {
        public string PartitionKey { get; private set; }
        public string RowKey { get; private set; }
        public string SheetName { get; private set; }
        public string ColumnName { get; private set; }
        public int RowIndex { get; private set; }
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


