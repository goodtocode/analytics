using GoodToCode.Shared.Blob.Abstractions;
using System.Collections.Generic;

namespace GoodToCode.Analytics.Ingress.Unit.Tests
{
    public class SheetFactory
    {
        public static SheetData CreateSheetData()
        {
            var cells = new List<CellData>() { CellFactory.CreateCellData() };
            var row = new List<RowData>() { new RowData(1, cells) };
            return new SheetData(0, "Sheet1", row, cells);
        }
    }
}
