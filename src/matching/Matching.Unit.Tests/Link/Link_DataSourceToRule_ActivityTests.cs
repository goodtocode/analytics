using GoodToCode.Analytics.Abstractions;
using GoodToCode.Analytics.Matching.Activities;
using GoodToCode.Analytics.Matching.Domain;
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
    public class Link_DataSourceToRule_ActivityTests
    {
        private readonly ILogger<Link_DataSourceToRule_ActivityTests> logItem;
        private readonly IConfiguration configuration;
        private readonly ExcelService excelService;
        private readonly StorageTablesServiceConfiguration configDestinationTable;

        private static string SutDataSourceFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/Matching-DataSource-Small.xlsx"; } }
        private static string SutRuleFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/Matching-Rule-Sequential.xlsx"; } }
        public ISheetData SutRules { get; private set; }
        public ISheetData SutDataSource { get; private set; }

        public Link_DataSourceToRule_ActivityTests()
        {
            logItem = LoggerFactory.CreateLogger<Link_DataSourceToRule_ActivityTests>();            
            configuration = AppConfigurationFactory.Create();
            excelService = ExcelServiceFactory.GetInstance().CreateExcelService();
            configDestinationTable = new StorageTablesServiceConfiguration(
                    configuration[AppConfigurationKeys.StorageTablesConnectionString],
                    $"UnitTests-{DateTime.UtcNow:yyyy-MM-dd}-Link");
        }

        [TestMethod]
        public async Task Link_DataSourceToRule_Activity()
        {
            Assert.IsTrue(File.Exists(SutDataSourceFile), $"{SutDataSourceFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            Assert.IsTrue(File.Exists(SutRuleFile), $"{SutRuleFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            try
            {
                // Load rules
                Stream ruleStream = new MemoryStream(await FileFactoryService.GetInstance().ReadAllBytesAsync(SutRuleFile));
                SutRules = excelService.GetSheet(ruleStream, 0);
                var matchingEntity = SutRules.ToMatchingRule();
                // Load data source
                Stream dataSourceStream = new MemoryStream(await FileFactoryService.GetInstance().ReadAllBytesAsync(SutDataSourceFile));
                SutDataSource = excelService.GetSheet(dataSourceStream, 0);
                var dataSourceRecords = new List<DataSourceEntity>();
                foreach (var row in SutDataSource.Rows)
                    dataSourceRecords.Add(new DataSourceEntity(row));
                var expressions = matchingEntity.ToFilterExpression<DataSourceEntity>();
                var linkResults = new LinkDataSourceToRuleActivity<DataSourceEntity>().Execute(expressions, dataSourceRecords);
                Assert.IsTrue(linkResults.MatchedData.Any(), "No results from filter service.");
                Assert.IsTrue(linkResults.MatchedRules.Any(), "No results from filter service.");
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

