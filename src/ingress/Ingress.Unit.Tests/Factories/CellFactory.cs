using GoodToCode.Analytics.Abstractions;
using GoodToCode.Shared.Blob.Abstractions;
using System;

namespace GoodToCode.Analytics.Ingress.Unit.Tests
{
    public class CellFactory
    {
        public static CellEntity CreateCellEntity()
        {
            return new CellEntity(Guid.NewGuid().ToString(), CreateCellData());
        }

        public static CellData CreateCellData()
        {
            return new CellData()
            {
                CellValue = "This is the Cell Value column.",
                ColumnIndex = 1,
                ColumnName = "Column1",
                RowIndex = 1,
                SheetIndex = 1,
                SheetName = "Sheet1",
                WorkbookName = "book.xlsx"
            };
        }
    }
}
