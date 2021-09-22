using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Blob.Excel;
using System.IO;

namespace GoodToCode.Analytics.Ingress.Activities
{
    public class ExcelSheetLoadActivity
    {
        private readonly INpoiService service;

        public ExcelSheetLoadActivity(INpoiService serviceExcel)
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
