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

namespace GoodToCode.Analytics.Matching.Unit.Tests
{
    [TestClass]
    public class Link_Multiple_Tests
    {
        private readonly ILogger<Link_Multiple_Tests> logItem;
        private readonly IConfiguration configuration;
        private readonly StorageTablesServiceConfiguration configDestination;
        private readonly IExcelService excelService;
        private static string SutDataSourceFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/Matching-DataSource-Small.xlsx"; } }
        private static string SutRuleFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/Matching-Rule-Multiple.xlsx"; } }
        public IEnumerable<string> RulePartitionKeys { get; private set; }
        public IWorkbookData SutRules { get; private set; }
        public IWorkbookData SutWorkbook { get; private set; }

        public Link_Multiple_Tests()
        {
            configuration = AppConfigurationFactory.Create();
            logItem = LoggerFactory.CreateLogger<Link_Multiple_Tests>();
            excelService = ExcelServiceFactory.GetInstance().CreateExcelService();
            configDestination = new StorageTablesServiceConfiguration(
                configuration[AppConfigurationKeys.StorageTablesConnectionString],
                $"UnitTest-{DateTime.UtcNow:yyyy-MM-dd}-{StorageTableNames.ResultsMultipleTable}");
        }

        [TestMethod]
        public async Task Link_Multiple_LinkDataSourceMultipleStep()
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
                    var workflowLink = new LinkDataSourceMultipleStep<DataSourceEntity>();
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
        public async Task Link_Multiple_PersistMatchResultStep()
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
                    var workflowLink = new LinkDataSourceMultipleStep<DataSourceEntity>();
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

        [TestCleanup]
        public void Cleanup()
        {
        }
    }
}

