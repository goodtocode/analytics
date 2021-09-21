using GoodToCode.Analytics.CognitiveServices.Activities;
using GoodToCodeAnalytics.CognitiveServices.Domain;
using GoodToCode.Shared.Analytics.Abstractions;
using GoodToCode.Shared.Analytics.CognitiveServices;
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

namespace GoodToCode.Analytics.CognitiveServices.Unit.Tests
{
    [TestClass]
    public class Healthcare_Persist_CloudTests
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<Healthcare_Persist_CloudTests> logItem;
        private readonly StorageTablesServiceConfiguration configStorage;
        private readonly CognitiveServiceConfiguration configText;
        private readonly INpoiService excelService;
        private string SutXlsxFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/OpinionFile.xlsx"; } }
        private int sheetToTransform = 0;
        private int colToTransform = 3;
        public RowEntity SutRow { get; private set; }
        public IEnumerable<RowEntity> SutRows { get; private set; }
        public Dictionary<string, StringValues> SutReturn { get; private set; }

        public Healthcare_Persist_CloudTests()
        {
            logItem = LoggerFactory.CreateLogger<Healthcare_Persist_CloudTests>();
            configuration = new AppConfigurationFactory().Create();
            configStorage = new StorageTablesServiceConfiguration(
                configuration[AppConfigurationKeys.StorageTablesConnectionString],
                $"UnitTest-{DateTime.UtcNow:yyyy-MM-dd}-Healthcare");
            configText = new CognitiveServiceConfiguration(
                configuration[AppConfigurationKeys.CognitiveServicesKeyCredential],
                configuration[AppConfigurationKeys.CognitiveServicesEndpoint]);
            excelService = new NpoiService();
        }

        [TestMethod]
        public async Task Healthcare_Persist_Cloud()       
        {
            Assert.IsTrue(File.Exists(SutXlsxFile), $"{SutXlsxFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            try
            {
                // Analyze
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutXlsxFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                var workflow = new HealthcareEntityAnalyzeActivity(excelService, new CognitiveServicesServiceFake());
                var results = await workflow.ExecuteAsync(itemToAnalyze, sheetToTransform, colToTransform);
                Assert.IsTrue(results.Any(), "No results from analytics service.");
                // Persist
                var persist = await new HealthcareEntityPersistActivity(configStorage).ExecuteAsync(results);
                Assert.IsTrue(persist.Any(), "No results from persistence service.");
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

