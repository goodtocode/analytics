using GoodToCode.Analytics.Abstractions;
using GoodToCode.Analytics.Matching.Activities;
using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Blob.Excel;
using GoodToCode.Shared.Persistence.StorageTables;
using Microsoft.Extensions.Configuration;
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
    public class Link_DataSourceSequential_Tests
    {
        private readonly ILogger<Link_DataSourceSequential_Tests> logItem;
        private readonly IConfiguration configuration;
        private readonly StorageTablesServiceConfiguration configRule;
        private readonly StorageTablesServiceConfiguration configDataSource;
        private readonly StorageTablesServiceConfiguration configDestination;
        private readonly IExcelService excelService;
        private static string SutDataSourceFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/Matching-DataSource-Small.xlsx"; } }
        private static string SutRuleFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/Matching-Rule-Sequential-Small.xlsx"; } }
        public IEnumerable<string> RulePartitionKeys { get; private set; }
        public IWorkbookData SutRules { get; private set; }
        public IWorkbookData SutWorkbook { get; private set; }


        public Link_DataSourceSequential_Tests()
        {
            configuration = AppConfigurationFactory.Create();
            logItem = LoggerFactory.CreateLogger<Link_DataSourceSequential_Tests>();
            excelService = ExcelServiceFactory.GetInstance().CreateExcelService();
            configRule = new StorageTablesServiceConfiguration(
                configuration[AppConfigurationKeys.StorageTablesConnectionString],
                Persist_Rules_ActivityTests.SutTable);
            configDataSource = new StorageTablesServiceConfiguration(
                configuration[AppConfigurationKeys.StorageTablesConnectionString],
                Persist_DataSource_ActivityTests.SutTable);
            configDestination = new StorageTablesServiceConfiguration(
                configuration[AppConfigurationKeys.StorageTablesConnectionString],
                $"UnitTest-{DateTime.UtcNow:yyyy-MM-dd}-LinkResultsSequence");
            RulePartitionKeys = new List<string>() { "Invalid", "ByAddressAndH2", "ByAddressAndH1", "ByAddressAndTitle", "ByAddress" };
        }

        [TestMethod]
        public async Task Link_DataSourceSequential_Activity()
        {
            Assert.IsTrue(File.Exists(SutDataSourceFile), $"{SutDataSourceFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            Assert.IsTrue(File.Exists(SutRuleFile), $"{SutRuleFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            try
            {
                // Load rules
                Stream ruleStream = new MemoryStream(await FileFactoryService.GetInstance().ReadAllBytesAsync(SutRuleFile));
                SutRules = excelService.GetWorkbook(ruleStream);
                var matchingEntity = SutRules.ToMatchingRule();
                // Load data source
                Stream dataSourceStream = new MemoryStream(await FileFactoryService.GetInstance().ReadAllBytesAsync(SutDataSourceFile));
                SutWorkbook = excelService.GetWorkbook(dataSourceStream, Path.GetFileName(SutRuleFile));
                foreach (var sheet in SutWorkbook.Sheets)
                {
                    var dataSourceRecords = new List<DataSourceEntity>();
                    foreach (var row in sheet.Rows)
                        dataSourceRecords.Add(new DataSourceEntity(row));
                    var workflowLink = new LinkDataSourceSequentialActivity<DataSourceEntity>();
                    var linkResults = workflowLink.Execute(matchingEntity, dataSourceRecords);
                    Assert.IsTrue(linkResults.Any(), "No results from filter service.");
                }
            }
            catch (Exception ex)
            {
                logItem.LogError(ex.Message, ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task Link_HtmlScrapeSequential_Orchestration()
        {
            Assert.IsTrue(File.Exists(SutDataSourceFile), $"{SutDataSourceFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            Assert.IsTrue(File.Exists(SutRuleFile), $"{SutRuleFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            try
            {
                // Load rules
                Stream ruleStream = new MemoryStream(await FileFactoryService.GetInstance().ReadAllBytesAsync(SutRuleFile));
                SutRules = excelService.GetWorkbook(ruleStream);
                var matchingEntity = SutRules.ToMatchingRule();
                // Load data source
                Stream dataSourceStream = new MemoryStream(await FileFactoryService.GetInstance().ReadAllBytesAsync(SutDataSourceFile));
                SutWorkbook = excelService.GetWorkbook(dataSourceStream, Path.GetFileName(SutRuleFile));
                foreach (var sheet in SutWorkbook.Sheets)
                {
                    var dataSourceRecords = new List<DataSourceEntity>();
                    foreach (var row in sheet.Rows)
                        dataSourceRecords.Add(new DataSourceEntity(row));
                    var workflowLink = new LinkDataSourceSequentialByGroupActivity<DataSourceEntity>(RulePartitionKeys);
                    var linkResults = workflowLink.Execute(matchingEntity, dataSourceRecords);
                    Assert.IsTrue(linkResults.Any(), "No results from filter service.");
                }
            }
            catch (Exception ex)
            {
                logItem.LogError(ex.Message, ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task Link_HtmlScrapeSequential_Persist()
        {
            Assert.IsTrue(File.Exists(SutDataSourceFile), $"{SutDataSourceFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            Assert.IsTrue(File.Exists(SutRuleFile), $"{SutRuleFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            try
            {
                // Load rules
                Stream ruleStream = new MemoryStream(await FileFactoryService.GetInstance().ReadAllBytesAsync(SutRuleFile));
                SutRules = excelService.GetWorkbook(ruleStream);
                var matchingEntity = SutRules.ToMatchingRule();
                // Load data source
                Stream dataSourceStream = new MemoryStream(await FileFactoryService.GetInstance().ReadAllBytesAsync(SutDataSourceFile));
                SutWorkbook = excelService.GetWorkbook(dataSourceStream, Path.GetFileName(SutRuleFile));
                foreach (var sheet in SutWorkbook.Sheets)
                {
                    var dataSourceRecords = sheet.ToDataSourceEntity();
                    var workflowLink = new LinkDataSourceSequentialByGroupActivity<DataSourceEntity>(RulePartitionKeys);
                    var linkResults = workflowLink.Execute(matchingEntity, dataSourceRecords);
                    Assert.IsTrue(linkResults.Any(), "No results from filter service.");
                    var workflowPersist = new PersistMatchResultActivity<DataSourceEntity>(configDestination);
                    var persistResults = await workflowPersist.ExecuteAsync(linkResults);
                    Assert.IsTrue(persistResults.Any(), "No results from filter service.");
                }
            }
            catch (Exception ex)
            {
                logItem.LogError(ex.Message, ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task Link_HtmlScrapeSequential_Storage()
        {            
            await new Persist_DataSource_ActivityTests().Ingress_DataSource_Orchestration();
            await new Persist_Rules_ActivityTests().Ingress_Rules_Orchestration();
            var rules = new List<MatchingRuleEntity>();
            foreach (var partitionKey in RulePartitionKeys)
                rules.AddRange(new StorageTablesService<MatchingRuleEntity>(configRule).GetAndCastItems(r => r.PartitionKey == partitionKey));
            var dataSource = new StorageTablesService<DataSourceEntity>(configDataSource).GetAndCastItems(r => r.PartitionKey != "");
            var workflowLink = new LinkDataSourceSequentialByGroupActivity<DataSourceEntity>(RulePartitionKeys);
            var linkResults = workflowLink.Execute(rules, dataSource);
            var workflowPersist = new PersistMatchResultActivity<DataSourceEntity>(configDestination);
            var Results = await workflowPersist.ExecuteAsync(linkResults);
        }

        [TestCleanup]
        public void Cleanup()
        {
        }
    }
}

