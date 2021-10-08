using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Blob.Excel;
using System.IO;

namespace GoodToCode.Analytics.Ingress.Activities
{
    public class ExcelSheetLoadActivity
    {
        private readonly IExcelService service;

        public ExcelSheetLoadActivity(IExcelService serviceExcel)
        {
            service = serviceExcel;
        }

        public ISheetData Execute(Stream excelStream, int sheetToAnalyze)
        {
            return service.GetSheet(excelStream, sheetToAnalyze);
        }
    }
}
