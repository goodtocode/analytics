﻿using GoodToCode.Analytics.Abstractions;
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
    public class Persist_DataSource_ActivityTests
    {
        private readonly ILogger<Persist_DataSource_ActivityTests> logItem;
        private readonly IConfiguration configuration;
        private readonly StorageTablesServiceConfiguration configStorage;
        private readonly IExcelService excelService;

        private static string SutDataSourceFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/Matching-DataSource-Small.xlsx"; } }
        public RowEntity SutRow { get; private set; }
        public IEnumerable<RowEntity> SutRows { get; private set; }
        public Dictionary<string, StringValues> SutReturn { get; private set; }
        public static string SutTable { get; } = $"UnitTest-{DateTime.UtcNow:yyyy-MM-dd}-{StorageTableNames.DataSourceTable}";

        public Persist_DataSource_ActivityTests()
        {
            configuration = AppConfigurationFactory.Create();
            logItem = LoggerFactory.CreateLogger<Persist_DataSource_ActivityTests>();
            excelService = ExcelServiceFactory.GetInstance().CreateExcelService();
            configStorage = new StorageTablesServiceConfiguration(
                configuration[AppConfigurationKeys.StorageTablesConnectionString],
                SutTable);
        }

        [TestMethod]
        public async Task Ingress_DataSource_Orchestration()
        {
            Assert.IsTrue(File.Exists(SutDataSourceFile), $"{SutDataSourceFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutDataSourceFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                var ruleSheet = excelService.GetSheet(itemToAnalyze, 0);
                var workflow = new PersistDataSourceActivity<DataSourceEntity>(configStorage);
                var results = await workflow.ExecuteAsync(ruleSheet.ToDataSourceEntity());
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

