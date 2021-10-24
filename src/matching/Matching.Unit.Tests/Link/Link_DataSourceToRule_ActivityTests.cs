using GoodToCode.Analytics.Abstractions;
using GoodToCode.Analytics.Matching.Activities;
using GoodToCode.Analytics.Matching.Domain;
using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Blob.Excel;
using GoodToCode.Shared.Persistence.StorageTables;
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
    public class Link_DataSourceToRule_ActivityTests
    {
        private readonly ILogger<Link_DataSourceToRule_ActivityTests> logItem;
        private readonly ExcelService excelService;
        private static string SutOpinionFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/OpinionFile.xlsx"; } }
        private static string SutDataSourceFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/Matching-DataSource-Small.xlsx"; } }
        private static string SutRuleFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/Matching-Rule-Sequential.xlsx"; } }
        public RowEntity SutRow { get; private set; }
        public IEnumerable<ICellData> SutSheet { get; private set; }
        public FilterExpression<ICellData> SutFilter { get; private set; }
        public Dictionary<string, StringValues> SutReturn { get; private set; }


        public Link_DataSourceToRule_ActivityTests()
        {
            logItem = LoggerFactory.CreateLogger<Link_DataSourceToRule_ActivityTests>();
            excelService = ExcelServiceFactory.GetInstance().CreateExcelService();
        }

        [TestMethod]
        public async Task Link_DataSourceToRule_Activity()
        {
            Assert.IsTrue(File.Exists(SutOpinionFile), $"{SutOpinionFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            SutFilter = new FilterExpression<ICellData>(x => x.ColumnIndex > -1);

            try
            {
                // Load rules
    //            var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutRuleFile);
    //            Stream itemToAnalyze = new MemoryStream(bytes);
    //            SutSheet = excelService.GetSheet(itemToAnalyze, 0);
    //            var matchingEntity = SutSheet.ToMatchingRule();
    //            // Load data source
    //            var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutOpinionFile);
    //            Stream itemToAnalyze = new MemoryStream(bytes);
    //            SutSheet = excelService.GetSheet(itemToAnalyze, 0).Cells;
    //            Results = new List<IEnumerable<IMatchingRuleEntity>>();
    //            List<FilterExpression<IHtmlScrapeEntity>> filters;
    //            MultiFilterActivity<IHtmlScrapeEntity> workflow;
    //            configStorage = new StorageTablesServiceConfiguration(
    //configuration[AppConfigurationKeys.StorageTablesConnectionString],
    //$"AI-{DateTime.UtcNow:yyyy-MM-dd}-KeyPhrase");

    //            // Link Invalid
    //            //filterRules = new List<FilterExpression<IHtmlScrapeEntity>>() {
    //            //    new FilterExpression<IHtmlScrapeEntity>(x => x.Status != "OK"),
    //            //    new FilterExpression<IHtmlScrapeEntity>(x => x.ContentType != "text/html; charset=utf-8")
    //            //};
    //            var ruleConfig = new StorageTablesServiceConfiguration(configDestinationTable.ConnectionString, $"matching-rules");
    //            var ruleClient = new StorageTablesService<MatchingRuleEntity>(ruleConfig);
    //            var rules = ruleClient.GetAndCastItems(x => x.PartitionKey == "RuleInvalid");
    //            // Convert to filterRules
    //            FilterExpression<IHtmlScrapeEntity> filter;
    //            rules
    //            foreach (var rule in rules)
    //            {
    //                switch (rule.MatchType)
    //                {
    //                    case "BeginsWith":
    //                        filter = new FilterExpression<IHtmlScrapeEntity>(x => x.GetType().GetProperty(rule.MatchColumn).GetValue(x, null).ToString().StartsWith(rule.MatchValue));
    //                        break;
    //                    case "EndsWith":
    //                        filter = new FilterExpression<IHtmlScrapeEntity>(x => x.GetType().GetProperty(rule.MatchColumn).GetValue(x, null).ToString().EndsWith(rule.MatchValue));
    //                        break;
    //                    case "Equals":
    //                        filter = new FilterExpression<IHtmlScrapeEntity>(x => x.GetType().GetProperty(rule.MatchColumn).GetValue(x, null).ToString() == rule.MatchValue);
    //                        break;
    //                    case "NotEquals":
    //                        filter = new FilterExpression<IHtmlScrapeEntity>(x => x.GetType().GetProperty(rule.MatchColumn).GetValue(x, null).ToString() != rule.MatchValue);
    //                        break;
    //                    case "Contains":
    //                    default:
    //                        filter = new FilterExpression<IHtmlScrapeEntity>(x => x.GetType().GetProperty(rule.MatchColumn).GetValue(x, null).ToString().Contains(rule.MatchValue));
    //                        break;
    //                }
    //            }
    //            var workflowLink = new LinkDataSourceToRuleActivity<IHtmlScrapeEntity>();
    //            var linkResults = workflowLink.Execute(filterRules, dataSource);

    //            Assert.IsTrue(results.Any(), "No results from filter service.");
    //            Assert.IsTrue(!string.IsNullOrWhiteSpace(results.FirstOrDefault()?.CellValue), "No results from filter service.");
            }
            catch (Exception ex)
            {
                logItem.LogError(ex.Message, ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task SingleFilter_Activity_Invalid()
        {
            Assert.IsTrue(File.Exists(SutDataSourceFile), $"{SutDataSourceFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            Assert.IsTrue(File.Exists(SutRuleFile), $"{SutRuleFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            SutFilter = new FilterExpression<ICellData>(x => x.ColumnName == "Status" && x.CellValue != "OK");

            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutDataSourceFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                SutSheet = excelService.GetSheet(itemToAnalyze, 0).Cells;
                var workflow = new SingleFilterActivity<ICellData>(SutFilter);
                var results = workflow.Execute(SutSheet);
                Assert.IsTrue(results.Any(), "No results from filter service.");
                Assert.IsTrue(!string.IsNullOrWhiteSpace(results.FirstOrDefault()?.CellValue), "No results from filter service.");
            }
            catch (Exception ex)
            {
                logItem.LogError(ex.Message, ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task SingleFilter_Activity_ByH2()
        {
            Assert.IsTrue(File.Exists(SutDataSourceFile), $"{SutDataSourceFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            Assert.IsTrue(File.Exists(SutRuleFile), $"{SutRuleFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            SutFilter = new FilterExpression<ICellData>(x => x.ColumnName == "Address" && x.CellValue.Contains("/bulk-discounts"));

            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutDataSourceFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                SutSheet = excelService.GetSheet(itemToAnalyze, 0).Cells;
                var workflow = new SingleFilterActivity<ICellData>(SutFilter);
                var results = workflow.Execute(SutSheet);
                Assert.IsTrue(results.Any(), "No results from filter service.");
                Assert.IsTrue(!string.IsNullOrWhiteSpace(results.FirstOrDefault()?.CellValue), "No results from filter service.");
            }
            catch (Exception ex)
            {
                logItem.LogError(ex.Message, ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task SingleFilter_Activity_ByH1()
        {
            Assert.IsTrue(File.Exists(SutDataSourceFile), $"{SutDataSourceFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            Assert.IsTrue(File.Exists(SutRuleFile), $"{SutRuleFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            SutFilter = new FilterExpression<ICellData>(x => x.ColumnName == "H1-1" && x.CellValue.StartsWith("Clinical Voices"));

            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutDataSourceFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                SutSheet = excelService.GetSheet(itemToAnalyze, 0).Cells;
                var workflow = new SingleFilterActivity<ICellData>(SutFilter);
                var results = workflow.Execute(SutSheet);
                Assert.IsTrue(results.Any(), "No results from filter service.");
                Assert.IsTrue(!string.IsNullOrWhiteSpace(results.FirstOrDefault()?.CellValue), "No results from filter service.");
            }
            catch (Exception ex)
            {
                logItem.LogError(ex.Message, ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task SingleFilter_Activity_ByTitle()
        {
            Assert.IsTrue(File.Exists(SutDataSourceFile), $"{SutDataSourceFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            Assert.IsTrue(File.Exists(SutRuleFile), $"{SutRuleFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            SutFilter = new FilterExpression<ICellData>(
                    x => x.ColumnName == "Title 1" && x.CellValue.StartsWith("Nurse Strong"));

            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutDataSourceFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                SutSheet = excelService.GetSheet(itemToAnalyze, 0).Cells;
                var workflow = new SingleFilterActivity<ICellData>(SutFilter);
                var results = workflow.Execute(SutSheet);
                Assert.IsTrue(results.Any(), "No results from filter service.");
                Assert.IsTrue(!string.IsNullOrWhiteSpace(results.FirstOrDefault()?.CellValue), "No results from filter service.");
            }
            catch (Exception ex)
            {
                logItem.LogError(ex.Message, ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task SingleFilter_Activity_ByAddress()
        {
            Assert.IsTrue(File.Exists(SutDataSourceFile), $"{SutDataSourceFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            Assert.IsTrue(File.Exists(SutRuleFile), $"{SutRuleFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            SutFilter = new FilterExpression<ICellData>(x => x.ColumnName == "Address" && x.CellValue != "/nursing-excellence/nurse-stories");

            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutDataSourceFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                SutSheet = excelService.GetSheet(itemToAnalyze, 0).Cells;
                var workflow = new SingleFilterActivity<ICellData>(SutFilter);
                var results = workflow.Execute(SutSheet);
                Assert.IsTrue(results.Any(), "No results from filter service.");
                Assert.IsTrue(!string.IsNullOrWhiteSpace(results.FirstOrDefault()?.CellValue), "No results from filter service.");
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

