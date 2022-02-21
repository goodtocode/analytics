using GoodToCode.Shared.Blob.Excel;

namespace GoodToCode.Analytics.Matching.Tests
{
    public class ExcelServiceFactory
    {
        private static readonly ExcelServiceFactory _excelFactoryInstance = new();
        private ExcelService service;

        private ExcelServiceFactory()
        {            
        }

        public static ExcelServiceFactory GetInstance() => _excelFactoryInstance;

        public ExcelService CreateExcelService()
        {
            service = new ExcelService();
            return service;
        }
    }
}
