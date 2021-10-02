using GoodToCode.Analytics.Abstractions;
using GoodToCode.Analytics.Matching.Activities;
using GoodToCode.Analytics.Matching.Domain;
using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Blob.Excel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.Matching.Unit.Tests
{
    [TestClass]
    public class Filter_SequentialFilter_ActivityTests
    {
        private readonly ILogger<Filter_SequentialFilter_ActivityTests> logItem;
        private readonly ExcelService excelService;
        private string SutOpinionFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/OpinionFile.xlsx"; } }
        private string SutDataSourceFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/Matching-DataSource-Small.xlsx"; } }
        private string SutRuleFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/Matching-Rule-Map.xlsx"; } }
        private string SutGoalFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/Matching-Goal-Categories.xlsx"; } }
        public RowEntity SutRow { get; private set; }
        public IEnumerable<ICellData> SutHeaders { get; private set; }
        public IEnumerable<FilterExpression<ICellData>> SutFilters { get; private set; }
        public Dictionary<string, StringValues> SutReturn { get; private set; }


        public Filter_SequentialFilter_ActivityTests()
        {
            logItem = LoggerFactory.CreateLogger<Filter_SequentialFilter_ActivityTests>();
            excelService = ExcelServiceFactory.GetInstance().CreateExcelService();
        }

        [TestMethod]
        public async Task SequentialFilter_Activity_OpinionFile()
        {
            Assert.IsTrue(File.Exists(SutOpinionFile), $"{SutOpinionFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            SutFilters = new List<FilterExpression<ICellData>>() { new FilterExpression<ICellData>(x => x.ColumnIndex > -1) };

            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutOpinionFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                SutHeaders = excelService.GetSheet(itemToAnalyze, 0).ToSheetData().GetRow(1).Cells;
                var workflow = new SequentialFilterActivity<ICellData>(SutFilters);
                var results = workflow.Execute(SutHeaders);
                Assert.IsTrue(results.Any(), "No results from filter service.");
            }
            catch (Exception ex)
            {
                logItem.LogError(ex.Message, ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task SequentialFilter_Activity_RuleFile()
        {
            Assert.IsTrue(File.Exists(SutDataSourceFile), $"{SutDataSourceFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            Assert.IsTrue(File.Exists(SutRuleFile), $"{SutRuleFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            Assert.IsTrue(File.Exists(SutGoalFile), $"{SutGoalFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            SutFilters = new List<FilterExpression<ICellData>>() { new FilterExpression<ICellData>(x => x.ColumnIndex > -1) };

            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutDataSourceFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                SutHeaders = excelService.GetSheet(itemToAnalyze, 0).ToSheetData().GetRow(1).Cells;
                var workflow = new SequentialFilterActivity<ICellData>(SutFilters);
                var results = workflow.Execute(SutHeaders);
                Assert.IsTrue(results.Any(), "No results from filter service.");
            }
            catch (Exception ex)
            {
                logItem.LogError(ex.Message, ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
        }
    }
}

