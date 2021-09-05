﻿using GoodToCode.Analytics.Activities;
using GoodToCode.Analytics.Domain;
using GoodToCode.Shared.Analytics.Abstractions;
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

namespace GoodToCode.Analytics.Unit.Tests
{
    [TestClass]
    public class Opinion_Analyze_FakeTests
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<Opinion_Analyze_FakeTests> logItem;
        private readonly StorageTablesServiceConfiguration configStorage;
        private readonly CognitiveServiceConfiguration configText;
        private string SutXlsxFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/AnalysisSimple.xlsx"; } }
        private int sheetToTransform = 0;
        private int colToTransform = 21;
        public RowEntity SutRow { get; private set; }
        public IEnumerable<RowEntity> SutRows { get; private set; }
        public Dictionary<string, StringValues> SutReturn { get; private set; }


        public Opinion_Analyze_FakeTests()
        {
            logItem = LoggerFactory.CreateLogger<Opinion_Analyze_FakeTests>();
            configuration = new AppConfigurationFactory().Create();
            configStorage = new StorageTablesServiceConfiguration(
                configuration[AppConfigurationKeys.StorageTablesConnectionString],
                $"UnitTest-{DateTime.UtcNow:yyyy-MM-dd_HH:mm}-Opinion");
            configText = new CognitiveServiceConfiguration(
                configuration[AppConfigurationKeys.CognitiveServicesKeyCredential],
                configuration[AppConfigurationKeys.CognitiveServicesEndpoint]);
        }

        [TestMethod]
        public async Task Opinion_Analyze_Fake()       
        {
            Assert.IsTrue(File.Exists(SutXlsxFile), $"{SutXlsxFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            try
            {
                // Analyze
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutXlsxFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                var workflow = new OpinionExtractActivity(new NpoiService(), new TextAnalyzerServiceFake());
                var results = await workflow.ExecuteAsync(itemToAnalyze, sheetToTransform, colToTransform);
                Assert.IsTrue(results.Any());
                // Persist
                var persist = await new OpinionPersistActivity(configStorage).ExecuteAsync(results);
                Assert.IsTrue(persist.Any());
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

