using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Blob.Excel;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.CognitiveServices.Activities
{
    public class ExcelColumnLoadActivity
    {
        private readonly INpoiService service;

        public ExcelColumnLoadActivity(INpoiService serviceExcel)
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
