using GoodToCode.Analytics.Domain;
using GoodToCode.Shared.Analytics.CognitiveServices;
using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Blob.Excel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.Activities
{
    public class HealthcareEntityAnalyzeActivity
    {
        private ICognitiveServicesService serviceAnalyzer;
        private INpoiService serviceExcel;

        public HealthcareEntityAnalyzeActivity(INpoiService serviceExcelReader, ICognitiveServicesService serviceCognitiveServices)
        {
            serviceAnalyzer = serviceCognitiveServices;
            serviceExcel = serviceExcelReader;
        }

        public async Task<IEnumerable<HealthcareNamedEntity>> ExecuteAsync(Stream excelStream, int sheetToAnalyze, int columnToAnalyze)
        {
            var returnValue = new List<HealthcareNamedEntity>();            

            var sheet = serviceExcel.GetWorkbook(excelStream).GetSheetAt(sheetToAnalyze);
            var sd = sheet.ToSheetData();
            var columnsToAnalyze = sd.GetColumn(columnToAnalyze);
            returnValue.AddRange(await new HealthcareEntityAnalyzeActivity(serviceExcel, serviceAnalyzer).ExecuteAsync(columnsToAnalyze));

            return returnValue;
        }

        public async Task<IEnumerable<HealthcareNamedEntity>> ExecuteAsync(IEnumerable<ICellData> cellsToAnalyze)
        {
            var returnValue = new List<HealthcareNamedEntity>();
            foreach (var column in cellsToAnalyze.Where(c => c.CellValue?.Length > 0))
                returnValue.AddRange(await new HealthcareEntityAnalyzeActivity(serviceExcel, serviceAnalyzer).ExecuteAsync(column));
            return returnValue;
        }

        public async Task<IEnumerable<HealthcareNamedEntity>> ExecuteAsync(ICellData cellToAnalyze)
        {
            var returnValue = new List<HealthcareNamedEntity>();
            if (cellToAnalyze.CellValue?.Length == 0) return returnValue;
            var analyzeResults = await serviceAnalyzer.ExtractHealthcareEntitiesAsync(cellToAnalyze.CellValue, "en-US");
            foreach(var result in analyzeResults)
                returnValue.Add(new HealthcareNamedEntity(cellToAnalyze, result));
            return returnValue;
        }
    }
}
