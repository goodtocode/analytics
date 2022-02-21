using GoodToCode.Analytics.Abstractions;
using GoodToCode.Shared.Blob.Abstractions;
using System;
using System.Collections.Generic;

namespace GoodToCode.Analytics.CognitiveServices.Tests
{
    public class RowEntityFactory
    {
        public static RowEntity CreateRowEntity()
        {
            var cell = new CellData()
            {
                CellValue = "",
                ColumnIndex = 1,
                ColumnName = "",
                RowIndex = 1,
                SheetIndex = 1,
                SheetName = "",
                WorkbookName = ""
            };

            var row = new RowEntity(Guid.NewGuid().ToString(), new List<ICellData>() { cell });
            return row;
        }
    }
}
