using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Blob.Excel;
using System.Collections.Generic;
using System.IO;

namespace GoodToCode.Analytics.Ingress.Activities
{
    public class ExcelWorkbookLoadActivity
    {
        private readonly IExcelService service;

        public ExcelWorkbookLoadActivity(IExcelService serviceExcel)
        {
            service = serviceExcel;
        }

        public IEnumerable<ISheetData> Execute(Stream excelStream)
        {
            var returnSheets = new List<ISheetData>();
            var wb = service.GetWorkbook(excelStream);

            foreach (var sheet in wb.SheetMetadata)
                returnSheets.Add(service.GetSheet(excelStream, sheet.SheetIndex));

            return returnSheets;
        }
    }
}
