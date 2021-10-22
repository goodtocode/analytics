using GoodToCode.Shared.Blob.Abstractions;
using System.Collections.Generic;

namespace GoodToCode.Analytics.Ingress.Unit.Tests
{
    public class WorkbookFactory
    {
        public static WorkbookData CreateWorkbookData()
        {
            var sheets = new List<SheetData>() { SheetFactory.CreateSheetData() };
            return new WorkbookData("book1.xls", sheets);
        }
    }
}
