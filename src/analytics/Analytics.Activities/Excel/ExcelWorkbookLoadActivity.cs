using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Blob.Excel;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.Activities
{
    public class ExcelWorkbookLoadActivity
    {
        private readonly INpoiService service;

        public ExcelWorkbookLoadActivity(INpoiService serviceExcel)
        {
            service = serviceExcel;
        }

        public IEnumerable<ISheetData> Execute(Stream excelStream)
        {
            var returnSheets = new List<ISheetData>();
            var wb = service.GetWorkbook(excelStream);

            for (int count = 0; count < wb.NumberOfSheets; count++)
                returnSheets.Add(wb.GetSheetAt(count).ToSheetData());                

            return returnSheets;
        }
    }
}
