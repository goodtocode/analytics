using GoodToCode.Analytics.Abstractions;
using GoodToCode.Shared.Blob.Abstractions;
using System;
using System.Collections.Generic;

namespace GoodToCode.Analytics.Ingress.Unit.Tests
{
    public class WorkbookFactory
    {
        public static WorkbookData CreateWorkbookData()
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
            var sheets = new List<SheetData>() { new SheetData(0, "Sheet1", row, cells) };
            return new WorkbookData("book1.xls", sheets);
        }
    }
}
