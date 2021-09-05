﻿//using GoodToCode.Analytics.Domain;
//using GoodToCode.Shared.Analytics.Abstractions;
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

//namespace GoodToCode.Analytics.Unit.Tests
//{
//    [TestClass]
//    public class EntityLink_Analytics_ActivityTests
//    {
//        private readonly IConfiguration configuration;
//        private readonly ILogger<StorageTablesItemService<RowEntity>> logItem;
//        private readonly StorageTablesServiceConfiguration configStorage;
//        private readonly CognitiveServiceConfiguration configText;
//        private string SutXlsxFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/AnalysisSimple.xlsx"; } }
//        private int sheetToTransform = 0;
//        private int colToTransform = 21;
//        public RowEntity SutRow { get; private set; }
//        public IEnumerable<RowEntity> SutRows { get; private set; }
//        public Dictionary<string, StringValues> SutReturn { get; private set; }

//        public EntityLink_Analytics_ActivityTests()
//        {
//            // Visual Studio vs. dotnet test vs. test runtimes execute different folders
//            logItem = LoggerFactory.CreateLogger<StorageTablesItemService<RowEntity>>();
//            configuration = new AppConfigurationFactory().Create();
//            configStorage = new StorageTablesServiceConfiguration(
//                configuration[AppConfigurationKeys.StorageTablesConnectionString],
//                $"UnitTest-{DateTime.UtcNow:yyyy-MM-dd_HH:mm}-EntityLink");
//            configText = new CognitiveServiceConfiguration(
//                configuration[AppConfigurationKeys.CognitiveServicesKeyCredential],
//                configuration[AppConfigurationKeys.CognitiveServicesEndpoint]);
//        }

//        [TestMethod]
//        public async Task EntityLink_Analytics_Activity()       
//        {
//            //Assert.IsTrue(File.Exists(SutXlsxFile), $"{SutXlsxFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

//            //try
//            //{ 
//            //    // Analyze
//            //    var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutXlsxFile);
//            //    Stream itemToAnalyze = new MemoryStream(bytes);
//            //    var workflow = new TextSentimentAnalyzeActivity(new TextAnalyzerServiceFake());
//            //    var results = await workflow.ExecuteAsync(itemToAnalyze, sheetToTransform, colToTransform);
//            //    Assert.IsTrue(results.Any());
//            //    // Persist
//            //    var persist = await new TextSentimentPersistActivity(configStorage).ExecuteAsync(results);
//            //    Assert.IsTrue(persist.Any());
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
//            //var service = new StorageTablesItemService<RowEntity>(configStorage);
//            //await service.DeleteTableAsync();
//        }
//    }
//}

