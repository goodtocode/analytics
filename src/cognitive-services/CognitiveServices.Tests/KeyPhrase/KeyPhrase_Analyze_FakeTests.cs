﻿using GoodToCode.Analytics.Abstractions;
using GoodToCode.Analytics.CognitiveServices.Activities;
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

namespace GoodToCode.Analytics.CognitiveServices.Tests
{
    [TestClass]
    public class KeyPhrase_Analyze_FakeTests
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<KeyPhrase_Analyze_FakeTests> logItem;
        private readonly StorageTablesServiceConfiguration configStorage;
        private static string SutXlsxFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/Sheet600.xlsx"; } }
        private readonly int sheetToTransform = 0;
        private readonly int colToTransform = 2;
        public RowEntity SutRow { get; private set; }
        public IEnumerable<RowEntity> SutRows { get; private set; }
        public Dictionary<string, StringValues> SutReturn { get; private set; }


        public KeyPhrase_Analyze_FakeTests()
        {
            logItem = LoggerFactory.CreateLogger<KeyPhrase_Analyze_FakeTests>();
            configuration = new AppConfigurationFactory().Create();
            configStorage = new StorageTablesServiceConfiguration(
                configuration[AppConfigurationKeys.StorageTablesConnectionString],
                $"UnitTest-{DateTime.UtcNow:yyyy-MM-dd}-KeyPhrase");
        }

        [TestMethod]
        public async Task KeyPhrase_Analyze_Fake()       
        {
            Assert.IsTrue(File.Exists(SutXlsxFile), $"{SutXlsxFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            try
            { 
                // Analyze
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutXlsxFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                var workflow = new  KeyPhraseExtractActivity(new ExcelService(), new TextAnalyzerServiceFake());
                var results = await workflow.ExecuteAsync(itemToAnalyze, sheetToTransform, colToTransform);
                Assert.IsTrue(results.Any(), "No results from analytics service.");
                // Persist
                var persist = await new KeyPhrasePersistActivity(configStorage).ExecuteAsync(results);
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

