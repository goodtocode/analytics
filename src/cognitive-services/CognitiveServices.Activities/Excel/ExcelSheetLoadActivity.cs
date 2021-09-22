using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Blob.Excel;
using System.IO;

namespace GoodToCode.Analytics.CognitiveServices.Activities
{
    public class ExcelSheetLoadActivity
    {
        private readonly IExcelService service;

        public ExcelSheetLoadActivity(IExcelService serviceExcel)
        {
            service = serviceExcel;
        }

        public SheetData Execute(Stream excelStream, int sheetToAnalyze)
        {
            var sheet = service.GetWorkbook(excelStream).GetSheetAt(sheetToAnalyze);
            return sheet.ToSheetData();
        }
    }
}
