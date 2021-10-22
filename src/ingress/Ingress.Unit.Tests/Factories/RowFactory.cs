using GoodToCode.Shared.Blob.Abstractions;
using System.Collections.Generic;

namespace GoodToCode.Analytics.Ingress.Unit.Tests
{
    public class RowFactory
    {
        public static RowData CreateRowData()
        {
            var cells = new List<CellData>() { CellFactory.CreateCellData() };
            return new RowData(0, cells);
        }
    }
}
