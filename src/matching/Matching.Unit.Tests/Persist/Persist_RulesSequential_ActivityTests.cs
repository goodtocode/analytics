using GoodToCode.Analytics.Abstractions;
using GoodToCode.Analytics.Matching.Activities;
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
    public class Persist_RulesSequential_ActivityTests
    {
        private readonly ILogger<Persist_RulesSequential_ActivityTests> logItem;
        private readonly IConfiguration configuration;
        private readonly StorageTablesServiceConfiguration configStorage;
        private readonly IExcelService excelService;

        private static string SutRuleFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/Matching-Rule-Sequential.xlsx"; } }
        public RowEntity SutRow { get; private set; }
        public IEnumerable<RowEntity> SutRows { get; private set; }
        public Dictionary<string, StringValues> SutReturn { get; private set; }
        public static string SutTable { get; } = $"UnitTest-{DateTime.UtcNow:yyyy-MM-dd}-{StorageTableNames.RuleSequentialTable}";

        public Persist_RulesSequential_ActivityTests()
        {
            configuration = AppConfigurationFactory.Create();
            logItem = LoggerFactory.CreateLogger<Persist_RulesSequential_ActivityTests>();
            excelService = ExcelServiceFactory.GetInstance().CreateExcelService();
            configStorage = new StorageTablesServiceConfiguration(
                configuration[AppConfigurationKeys.StorageTablesConnectionString],
                SutTable);
        }

        [TestMethod]
        public async Task Ingress_RulesSequential_Orchestration()
        {
            Assert.IsTrue(File.Exists(SutRuleFile), $"{SutRuleFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutRuleFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                var ruleSheet = excelService.GetWorkbook(itemToAnalyze);
                var workflow = new PersistMatchingRuleActivity(configStorage);
                var results = await workflow.ExecuteAsync(ruleSheet.ToMatchingRule());
                Assert.IsTrue(results.Any(), "No results from Excel service.");
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

