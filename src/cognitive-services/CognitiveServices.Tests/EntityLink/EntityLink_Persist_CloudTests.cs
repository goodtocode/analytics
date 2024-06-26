﻿//using GoodToCode.Analytics.CognitiveServices.Domain;
//using GoodToCode.Shared.TextAnalytics.Abstractions;
//using GoodToCode.Shared.Persistence.StorageTables;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Primitives;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Reflection;
//using System.Threading.Tasks;

//namespace GoodToCode.Analytics.CognitiveServices.Tests
//{
//    [TestClass]
//    public class EntityLink_Persist_ActivityTests
//    {
//        private readonly IConfiguration configuration;
//        private readonly ILogger<StorageTablesService<RowEntity>> logItem;
//        private readonly StorageTablesServiceConfiguration configStorage;
//        private readonly CognitiveServiceConfiguration configText;
//        private static string SutXlsxFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/OpinionFile.xlsx"; } }
//        private readonly int sheetToTransform = 0;
//        private readonly int colToTransform = 3;
//        public RowEntity SutRow { get; private set; }
//        public IEnumerable<RowEntity> SutRows { get; private set; }
//        public Dictionary<string, StringValues> SutReturn { get; private set; }

//        public EntityLink_Persist_ActivityTests()
//        {
//            logItem = LoggerFactory.CreateLogger<StorageTablesService<RowEntity>>();
//            configuration = new AppConfigurationFactory().Create();
//            configStorage = new StorageTablesServiceConfiguration(
//                configuration[AppConfigurationKeys.StorageTablesConnectionString],
//                $"UnitTest-{DateTime.UtcNow:yyyy-MM-dd}-EntityLink");
//            configText = new CognitiveServiceConfiguration(
//                configuration[AppConfigurationKeys.CognitiveServicesKeyCredential],
//                configuration[AppConfigurationKeys.CognitiveServicesEndpoint]);
//        }

//        [TestMethod]
//        public async Task EntityLink_Persist_Cloud()       
//        {
//            //Assert.IsTrue(File.Exists(SutXlsxFile), $"{SutXlsxFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

//            //try
//            //{ 
//            //    // Analyze
//            //    var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutXlsxFile);
//            //    Stream itemToAnalyze = new MemoryStream(bytes);
//            //    var workflow = new TextSentimentAnalyzeActivity(new TextAnalyzerServiceFake());
//            //    var results = await workflow.ExecuteAsync(itemToAnalyze, sheetToTransform, colToTransform);
//            //    Assert.IsTrue(results.Any(), "No results from analytics service.");
//            //    // Persist
//            //    var persist = await new TextSentimentPersistActivity(configStorage).ExecuteAsync(results);
//            //    Assert.IsTrue(persist.Any(), "No results from persistence service.");
//            //}
//            //catch (Exception ex)
//            //{
//            //    logItem.LogError(ex.Message, ex);
//            //    Assert.Fail(ex.Message);
//            //}
//        }

//        [TestCleanup]
//        public void Cleanup()
//        {
//            //var service = new StorageTablesService<RowEntity>(configStorage);
//            //await service.DeleteTableAsync();
//        }
//    }
//}

