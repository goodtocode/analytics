using GoodToCode.Analytics.CognitiveServices.Activities;
using GoodToCode.Analytics.CognitiveServices.Domain;
using GoodToCode.Shared.TextAnalytics.Abstractions;
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
    public class Sentiment_Analyze_FakeTests
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<Sentiment_Analyze_FakeTests> logItem;
        private readonly CognitiveServiceConfiguration configText;
        private string SutXlsxFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/Sheet600.xlsx"; } }
        private readonly int sheetToTransform = 0;
        private readonly int colToTransform = 1;
        public RowEntity SutRow { get; private set; }
        public IEnumerable<RowEntity> SutRows { get; private set; }
        public Dictionary<string, StringValues> SutReturn { get; private set; }

        public Sentiment_Analyze_FakeTests()
        {
            logItem = LoggerFactory.CreateLogger<Sentiment_Analyze_FakeTests>();
            configuration = new AppConfigurationFactory().Create();
            configText = new CognitiveServiceConfiguration(
                configuration[AppConfigurationKeys.CognitiveServicesKeyCredential],
                configuration[AppConfigurationKeys.CognitiveServicesEndpoint]);
        }

        [TestMethod]
        public async Task Sentiment_Analyze_Fake()       
        {
            Assert.IsTrue(File.Exists(SutXlsxFile), $"{SutXlsxFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            try
            { 
                // Analyze
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutXlsxFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                var workflow = new  SentimentAnalyzeActivity(new ExcelService(), new TextAnalyzerServiceFake());
                var results = await workflow.ExecuteAsync(itemToAnalyze, sheetToTransform, colToTransform);
                Assert.IsTrue(results.Any(), "No results from analytics service.");
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

