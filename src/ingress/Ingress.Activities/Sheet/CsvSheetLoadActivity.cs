using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Blob.Csv;
using System.IO;

namespace GoodToCode.Analytics.Ingress.Activities
{
    public class CsvSheetLoadActivity
    {
        private readonly ICsvService service;

        public CsvSheetLoadActivity(ICsvService serviceCsv)
        {
            service = serviceCsv;
        }

        public ISheetData Execute(Stream CsvStream)
        {
            var sheet = service.GetSheet(CsvStream);
            return sheet;
        }
    }
}
