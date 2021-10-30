using GoodToCode.Analytics.Abstractions;
using GoodToCode.Shared.Blob.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace GoodToCode.Analytics.Abstractions
{
    public class RowEntity : IRowEntity
    {
        [JsonInclude]
        public string PartitionKey { get; }
        [JsonInclude]
        public string RowKey { get; }
        public string WorkbookName { get; private set; }
        public int SheetIndex { get; private set; }
        [JsonInclude]
        public string SheetName { get; }
        [JsonInclude]
        public int RowIndex { get; }
        [JsonInclude]
        public IEnumerable<ICellData> Cells { get; }

        public RowEntity() { }
        public RowEntity(Guid rowKey, ICellData cell) : this(rowKey.ToString(), cell) { }
        public RowEntity(string rowKey, ICellData cell)
        {
            RowKey = rowKey;
            Cells = new List<ICellData>() { cell };
            PartitionKey = cell.SheetName;
            WorkbookName = cell.WorkbookName;
            SheetIndex = cell.SheetIndex;
            SheetName = cell.SheetName;
            RowIndex = cell.RowIndex;
        }        

        public RowEntity(string rowKey, IEnumerable<ICellData> cells)
        {
            RowKey = rowKey;
            Cells = cells;
            var firstCell = cells.FirstOrDefault();
            PartitionKey = firstCell.SheetName;
            WorkbookName = firstCell.WorkbookName;
            SheetIndex = firstCell.SheetIndex;
            SheetName = firstCell.SheetName;
            RowIndex = firstCell.RowIndex;
        }

        public RowEntity(string partitionKey, string rowKey, IEnumerable<ICellData> cells) : this(rowKey, cells) 
        {
            PartitionKey = partitionKey;
        }


        public RowEntity(IEnumerable<ICellData> cells) : this(Guid.NewGuid().ToString(), cells)
        {
        }

        public Dictionary<string, object> ToDictionary()
        {            
            var rootObj = this.GetType()
                            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                .Where(x => x.PropertyType.IsPrimitive || x.PropertyType.IsValueType || x.PropertyType == typeof(Guid) || x.PropertyType == typeof(string))
                            .ToDictionary(prop => prop.Name, prop => (object)prop.GetValue(this, null));
            var cells = Cells.ToDictionary(k => k.ColumnName, v => (object)v.CellValue);
            var returnDict = rootObj.Concat(cells.Where(kvp => !rootObj.ContainsKey(kvp.Key))).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            return returnDict;
        }
    }
}


    