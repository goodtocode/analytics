using GoodToCode.Analytics.Abstractions;
using GoodToCode.Analytics.Matching.Activities;
using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Blob.Excel;
using GoodToCode.Shared.Persistence.StorageTables;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.Matching.Tests
{
    [TestClass]
    public class Link_Sequential_Tests
    {
        private readonly ILogger<Link_Sequential_Tests> logItem;
        private readonly IConfiguration configuration;
        private readonly StorageTablesServiceConfiguration configRule;
        private readonly StorageTablesServiceConfiguration configDataSource;
        private readonly StorageTablesServiceConfiguration configDestination;
        private readonly IExcelService excelService;
        private static string SutDataSourceFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/03-Matching-DataSource-Small.xlsx"; } }
        private static string SutRuleFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/04-Matching-Rule-Sequential.xlsx"; } }
        public IEnumerable<string> RulePartitionKeys { get; private set; }
        public IWorkbookData SutRules { get; private set; }
        public IWorkbookData SutWorkbook { get; private set; }

        public Link_Sequential_Tests()
        {
            configuration = new AppConfigurationFactory().Create();
            logItem = LoggerFactory.CreateLogger<Link_Sequential_Tests>();
            excelService = ExcelServiceFactory.GetInstance().CreateExcelService();
            configRule = new StorageTablesServiceConfiguration(
                configuration[AppConfigurationKeys.StorageTablesConnectionString],
                Persist_RulesSequential_StepTests.SutTable);
            configDataSource = new StorageTablesServiceConfiguration(
                configuration[AppConfigurationKeys.StorageTablesConnectionString],
                Persist_DataSource_StepTests.SutTable);
            configDestination = new StorageTablesServiceConfiguration(
                configuration[AppConfigurationKeys.StorageTablesConnectionString],
                $"UnitTest-{DateTime.UtcNow:yyyy-MM-dd}-{StorageTableNames.ResultsSequentialTable}");
        }

        [TestMethod]
        public async Task Link_Sequential_LinkDataSourceSequentialStep()
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
                    var workflowLink = new LinkDataSourceSequentialStep<DataSourceEntity>();
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
        public async Task Link_Sequential_PersistMatchResultStep()
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
                    var workflowLink = new LinkDataSourceSequentialStep<DataSourceEntity>();
                    var linkResults = workflowLink.Execute(matchingEntity, dataSourceRecords);
                    Assert.IsTrue(linkResults.Any(), "No results from filter service.");
                    var workflowPersist = new PersistMatchResultStep<DataSourceEntity>(configDestination);
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
        public async Task Link_Sequential_Storage()
        {
            await new Persist_DataSource_StepTests().Ingress_DataSource_OrchestrationFake();
            await new Persist_RulesSequential_StepTests().Ingress_RulesSequential_OrchestrationFake();
            var rules = new StorageTablesService<MatchingRuleEntity>(configRule).GetAndCastItems(r => r.PartitionKey != "");
            var dataSource = new StorageTablesService<DataSourceEntity>(configDataSource).GetAndCastItems(r => r.PartitionKey != "");
            var workflowLink = new LinkDataSourceSequentialStep<DataSourceEntity>();
            var linkResults = workflowLink.Execute(rules, dataSource);
            var workflowPersist = new PersistMatchResultStep<DataSourceEntity>(configDestination);

            var results = await workflowPersist.ExecuteAsync(linkResults);
            Assert.IsTrue(results.Any());
        }

        [TestCleanup]
        public void Cleanup()
        {
        }
    }
}

