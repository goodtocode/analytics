using GoodToCode.Shared.TextAnalytics.Abstractions;
using GoodToCode.Shared.TextAnalytics.CognitiveServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace GoodToCode.Shared.TextAnalytics
{
    [TestClass]
    public class Configuration_OptionsPatternTests
    {
        private IConfiguration configuration;
        private ILogger<TextAnalyzerService> log;
        private CognitiveServiceOptions config;
        private TextAnalyzerService service;

        public string SutText { get; set; }

        public Configuration_OptionsPatternTests() { }

        [TestInitialize]
        public void Initialize()
        {
            log = LoggerFactory.CreateLogger<TextAnalyzerService>();
            configuration = new AppConfigurationFactory().Create();
            config = new CognitiveServiceOptions(
                configuration[AppConfigurationKeys.TextAnalyticsKeyCredential],
                configuration[AppConfigurationKeys.TextAnalyticsEndpoint]);
            service = new TextAnalyzerService(config);
        }

        [TestMethod]
        public void Configuration_CognitiveServicesOptions()
        {
            var section = configuration.GetSection(AppConfigurationKeys.TextAnalyticsSection);
            // Azure App Configuration does not support sections
            Assert.IsTrue(section?.Value == null);
        }
    }
}
