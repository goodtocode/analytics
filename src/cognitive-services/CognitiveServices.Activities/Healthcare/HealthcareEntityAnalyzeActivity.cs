using GoodToCode.Analytics.CognitiveServices.Domain;
using GoodToCode.Shared.Analytics.CognitiveServices;
using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Blob.Excel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.CognitiveServices.Activities
{
    public class HealthcareEntityAnalyzeActivity
    {
        private ICognitiveServicesService serviceAnalyzer;
        private IExcelService serviceExcel;

        public HealthcareEntityAnalyzeActivity(IExcelService serviceExcelReader, ICognitiveServicesService serviceCognitiveServices)
        {
            serviceAnalyzer = serviceCognitiveServices;
            serviceExcel = serviceExcelReader;
        }

        public async Task<IEnumerable<HealthcareNamedEntity>> ExecuteAsync(Stream excelStream, int sheetToAnalyze, int columnToAnalyze)
        {
            var returnValue = new List<HealthcareNamedEntity>();            

            var sheet = serviceExcel.GetWorkbook(excelStream).GetSheetAt(sheetToAnalyze);
            var sd = sheet.ToSheetData();
            var cellsToAnalyze = sd.GetColumn(columnToAnalyze);
            returnValue.AddRange(await new HealthcareEntityAnalyzeActivity(serviceExcel, serviceAnalyzer).ExecuteAsync(cellsToAnalyze));

            return returnValue;
        }

        public async Task<IEnumerable<HealthcareNamedEntity>> ExecuteAsync(IEnumerable<ICellData> cellsToAnalyze)
        {
            var returnValue = new List<HealthcareNamedEntity>();
            foreach (var cell in cellsToAnalyze.Where(c => string.IsNullOrEmpty(c.CellValue) == false))
                returnValue.AddRange(await new HealthcareEntityAnalyzeActivity(serviceExcel, serviceAnalyzer).ExecuteAsync(cell));
            return returnValue;
        }

        public async Task<IEnumerable<HealthcareNamedEntity>> ExecuteAsync(ICellData cellToAnalyze)
        {
            var returnValue = new List<HealthcareNamedEntity>();
            if (string.IsNullOrWhiteSpace(cellToAnalyze?.CellValue)) return returnValue;
            var analyzeResults = await serviceAnalyzer.ExtractHealthcareEntitiesAsync(cellToAnalyze.CellValue, "en-US");
            foreach(var result in analyzeResults)
                returnValue.Add(new HealthcareNamedEntity(cellToAnalyze, result));
            return returnValue;
        }
    }
}
