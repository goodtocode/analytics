using GoodToCode.Analytics.Abstractions;
using GoodToCode.Shared.Blob.Abstractions;
using System;
using System.Collections.Generic;

namespace GoodToCode.Analytics.Ingress.Unit.Tests
{
    public class SheetFactory
    {
        public static SheetData CreateSheetData()
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

            var cells = new List<CellData>() { cell };
            var row = new List<RowData>() { new RowData(1, cells) };
            return new SheetData(0, "Sheet1", row, cells);
        }
    }
}
