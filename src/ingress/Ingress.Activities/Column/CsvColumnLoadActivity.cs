using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Blob.Csv;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.Ingress.Activities
{
    public class CsvColumnLoadActivity
    {
        private readonly ICsvService service;

        public CsvColumnLoadActivity(ICsvService serviceCsv)
        {
            service = serviceCsv;
        }

        public  IEnumerable<ICellData> Execute(Stream CsvStream, int columnToAnalyze)
        {
            return service.GetColumn(CsvStream, columnToAnalyze);
        }
    }
}
