using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Blob.Excel;
using System.Collections.Generic;
using System.IO;

namespace GoodToCode.Analytics.Ingress.Activities
{
    public class ExcelColumnLoadActivity
    {
        private readonly IExcelService service;

        public ExcelColumnLoadActivity(IExcelService serviceExcel)
        {
            service = serviceExcel;
        }

        public  IEnumerable<ICellData> Execute(Stream excelStream, int sheetToAnalyze, int columnToAnalyze)
        {
            var sheet = service.GetWorkbook(excelStream).GetSheetAt(sheetToAnalyze);
            var sd = sheet.ToSheetData();
            
            return sd.GetColumn(columnToAnalyze);
        }
    }
}
