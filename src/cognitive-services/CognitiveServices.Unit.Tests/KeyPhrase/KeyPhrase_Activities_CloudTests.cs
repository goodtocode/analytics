using GoodToCode.Analytics.Abstractions;
using GoodToCode.Analytics.CognitiveServices.Activities;
using GoodToCode.Shared.Blob.Excel;
using GoodToCode.Shared.TextAnalytics.Abstractions;
using GoodToCode.Shared.TextAnalytics.CognitiveServices;
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
    public class KeyPhrase_Activities_CloudTests
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<KeyPhrase_Activities_CloudTests> logItem;
        
        private readonly CognitiveServiceConfiguration configText;
        private readonly IExcelService excelService;
        private readonly ITextAnalyzerService analyzerService;
        private static string SutXlsxFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/OpinionFile.xlsx"; } }
        private readonly string sheetName = "TestSheet1";
        private readonly string sheetSearchChar = "*";
        public RowEntity SutRow { get; private set; }
        public IEnumerable<RowEntity> SutRows { get; private set; }
        public Dictionary<string, StringValues> SutReturn { get; private set; }

        public KeyPhrase_Activities_CloudTests()
        {
            logItem = LoggerFactory.CreateLogger<KeyPhrase_Activities_CloudTests>();
            configuration = new AppConfigurationFactory().Create();
            configText = new CognitiveServiceConfiguration(
                configuration[AppConfigurationKeys.CognitiveServicesKeyCredential],
                configuration[AppConfigurationKeys.CognitiveServicesEndpoint]);
            excelService = new ExcelService();
            analyzerService = new TextAnalyzerService(configText);
        }

        [TestMethod]
        public async Task Free_KeyPhrase_Search_Extract()
        {
            Assert.IsTrue(File.Exists(SutXlsxFile), $"{SutXlsxFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutXlsxFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                var columns = new ExcelColumnSearchActivity(excelService).Execute(itemToAnalyze, sheetName, sheetSearchChar);
                var returnedItem = await new KeyPhraseExtractActivity(excelService, analyzerService).ExecuteAsync(columns);
                Assert.IsTrue(returnedItem.Any());
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

