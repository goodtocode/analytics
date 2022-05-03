using GoodToCode.Shared.TextAnalytics.Abstractions;
using GoodToCode.Shared.TextAnalytics.CognitiveServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GoodToCode.Shared.TextAnalytics
{
    [TestClass]
    public class CognitiveServicesTests
    {
        private IConfiguration configuration;
        private ILogger<CognitiveServicesService> log;
        private CognitiveServiceOptions config;
        private CognitiveServicesService service;

        public string SutText { get; set; }

        public CognitiveServicesTests() { }

        [TestInitialize]
        public void Initialize()
        {
            log = LoggerFactory.CreateLogger<CognitiveServicesService>();
            configuration = new AppConfigurationFactory().Create();
            config = new CognitiveServiceOptions(
                configuration[AppConfigurationKeys.CognitiveServicesKeyCredential],
                configuration[AppConfigurationKeys.CognitiveServicesEndpoint]);
            service = new CognitiveServicesService(config);
        }

        [TestMethod]
        public async Task CognitiveServices_Language()
        {
            SutText = "Ce document est rédigé en Français.";
            var sutResult = await service.DetectLanguageAsync(SutText);
            Assert.IsTrue(sutResult != null);
            Assert.IsTrue(sutResult.Length > 0);
        }

        [TestMethod]
        public async Task CognitiveServices_KeyPhrase()
        {
            SutText = "My cat might need to see a veterinarian.";
            var sutResult = await service.ExtractKeyPhrasesAsync(SutText);
            Assert.IsTrue(sutResult != null);
            Assert.IsTrue(sutResult.Any());
        }

        [TestMethod]
        public async Task CognitiveServices_Healthcare()
        {
            SutText = @"RECORD #333582770390100 | MH | 85986313 | | 054351 | 2/14/2001 12:00:00 AM | CORONARY ARTERY DISEASE | Signed | DIS | \
                    Admission Date: 5/22/2001 Report Status: Signed Discharge Date: 4/24/2001 ADMISSION DIAGNOSIS: CORONARY ARTERY DISEASE. \
                    HISTORY OF PRESENT ILLNESS: The patient is a 54-year-old gentleman with a history of progressive angina over the past several months. \
                    The patient had a cardiac catheterization in July of this year revealing total occlusion of the RCA and 50% left main disease ,\
                    with a strong family history of coronary artery disease with a brother dying at the age of 52 from a myocardial infarction and \
                    another brother who is status post coronary artery bypass grafting. The patient had a stress echocardiogram done on July , 2001 , \
                    which showed no wall motion abnormalities , but this was a difficult study due to body habitus. The patient went for six minutes with \
                    minimal ST depressions in the anterior lateral leads , thought due to fatigue and wrist pain , his anginal equivalent. Due to the patient's \
                    increased symptoms and family history and history left main disease with total occasional of his RCA was referred for revascularization with open heart surgery.";
            var sutResult = await service.ExtractHealthcareEntitiesAsync(SutText);
            Assert.IsTrue(sutResult != null);
            Assert.IsTrue(sutResult.Any());
            var sutFirst = sutResult.FirstOrDefault();
            Assert.IsTrue(sutFirst.Category.Length > 0);
            Assert.IsTrue(sutFirst.AnalyzedText.Length > 0);
        }

        [TestMethod]
        public async Task CognitiveServices_Entities()
        {
            SutText = "I had a wonderful trip to Seattle last week.";
            var sutResult = await service.ExtractEntitiesAsync(SutText);
            Assert.IsTrue(sutResult != null);
            Assert.IsTrue(sutResult.Any());
            var sutFirst = sutResult.FirstOrDefault();
            Assert.IsTrue(sutFirst.Category.Length > 0);
            Assert.IsTrue(sutFirst.AnalyzedText.Length > 0);
            Assert.IsTrue(sutFirst.Confidence > -1);
        }

        [TestMethod]
        public async Task CognitiveServices_Links()
        {
            SutText = @"Microsoft was founded by Bill Gates and Paul Allen on April 4, 1975, \
                to develop and sell BASIC interpreters for the Altair 8800. \
                During his career at Microsoft, Gates held the positions of chairman, \
                chief executive officer, president and chief software architect, \
                while also being the largest individual shareholder until May 2014.";
            var sutResult = await service.ExtractEntityLinksAsync(SutText);
            Assert.IsTrue(sutResult != null);
            Assert.IsTrue(sutResult.Any());
            var sutFirst = sutResult.FirstOrDefault();
            Assert.IsTrue(sutFirst.Matches.Any());
            Assert.IsTrue(sutFirst.Name.Length > 0);
        }

        [TestMethod]
        public async Task CognitiveServices_Opinion()
        {
            SutText = "The food and service were unacceptable, but the concierge were nice.";
            var sutResult = await service.ExtractOpinionAsync(SutText);
            Assert.IsTrue(sutResult != null);
            Assert.IsTrue(sutResult.Any());
            var sutFirst = sutResult.FirstOrDefault();
            Assert.IsTrue(sutFirst.DocumentSentiment != null);
            Assert.IsTrue(sutFirst.OpinionSentiments != null);
            Assert.IsTrue(sutFirst.SentenceOpinion != null);
            Assert.IsTrue(sutFirst.SentenceSentiment != null);
        }

        [TestMethod]
        public async Task CognitiveServices_Sentiment()
        {
            SutText = "I had the best day of my life. I wish you were there with me.";
            var sutResult = await service.AnalyzeSentimentAsync(SutText);
            Assert.IsTrue(sutResult != null);
        }
    }
}
