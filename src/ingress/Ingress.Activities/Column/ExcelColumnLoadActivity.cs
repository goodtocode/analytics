using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Blob.Excel;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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
            return service.GetColumn(excelStream, sheetToAnalyze, columnToAnalyze);
        }
    }
}
