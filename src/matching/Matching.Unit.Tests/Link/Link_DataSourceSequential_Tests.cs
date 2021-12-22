﻿using GoodToCode.Analytics.Abstractions;
using GoodToCode.Analytics.Matching.Activities;
using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Blob.Excel;
using GoodToCode.Shared.Persistence.StorageTables;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
    public class Link_DataSourceSequential_Tests
    {
        private readonly ILogger<Link_DataSourceSequential_Tests> logItem;
        private readonly IConfiguration configuration;
        private readonly StorageTablesServiceConfiguration configRule;
        private readonly StorageTablesServiceConfiguration configDataSource;
        private readonly StorageTablesServiceConfiguration configDestination;
        private readonly IExcelService excelService;
        private static string SutDataSourceFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/Matching-DataSource-Small.xlsx"; } }
        private static string SutRuleFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/Matching-Rule-Sequential.xlsx"; } }
        public IEnumerable<string> RulePartitionKeys { get; private set; }
        public IWorkbookData SutRules { get; private set; }
        public IWorkbookData SutWorkbook { get; private set; }


        public Link_DataSourceSequential_Tests()
        {
            configuration = AppConfigurationFactory.Create();
            logItem = LoggerFactory.CreateLogger<Link_DataSourceSequential_Tests>();
            excelService = ExcelServiceFactory.GetInstance().CreateExcelService();
            configRule = new StorageTablesServiceConfiguration(
                configuration[AppConfigurationKeys.StorageTablesConnectionString],
                Persist_RulesSequential_ActivityTests.SutTable);
            configDataSource = new StorageTablesServiceConfiguration(
                configuration[AppConfigurationKeys.StorageTablesConnectionString],
                Persist_DataSource_ActivityTests.SutTable);
            configDestination = new StorageTablesServiceConfiguration(
                configuration[AppConfigurationKeys.StorageTablesConnectionString],
                $"UnitTest-{DateTime.UtcNow:yyyy-MM-dd}-{StorageTableNames.ResultsSequentialTable}");
            RulePartitionKeys = new List<string>() { "Invalid", "ByAddressAndH2", "ByAddressAndH1", "ByAddressAndTitle", "ByAddress-3", "ByAddress-2", "ByAddress-1", "NotMatched" };
        }

        [TestMethod]
        public async Task Link_DataSourceSequential_Activity()
        {
            Assert.IsTrue(File.Exists(SutDataSourceFile), $"{SutDataSourceFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            Assert.IsTrue(File.Exists(SutRuleFile), $"{SutRuleFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            try
            {
                // Load rules
                Stream ruleStream = new MemoryStream(await FileFactoryService.GetInstance().ReadAllBytesAsync(SutRuleFile));
                SutRules = excelService.GetWorkbook(ruleStream);
                var matchingEntity = SutRules.ToMatchingRule();
                // Load data source
                Stream dataSourceStream = new MemoryStream(await FileFactoryService.GetInstance().ReadAllBytesAsync(SutDataSourceFile));
                SutWorkbook = excelService.GetWorkbook(dataSourceStream, Path.GetFileName(SutRuleFile));
                foreach (var sheet in SutWorkbook.Sheets)
                {
                    var dataSourceRecords = new List<DataSourceEntity>();
                    foreach (var row in sheet.Rows)
                        dataSourceRecords.Add(new DataSourceEntity(row));
                    var workflowLink = new LinkDataSourceSequentialActivity<DataSourceEntity>();
                    var linkResults = workflowLink.Execute(matchingEntity, dataSourceRecords);
                    Assert.IsTrue(linkResults.Any(), "No results from filter service.");
                }
            }
            catch (Exception ex)
            {
                logItem.LogError(ex.Message, ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task Link_HtmlScrapeSequential_Orchestration()
        {
            Assert.IsTrue(File.Exists(SutDataSourceFile), $"{SutDataSourceFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            Assert.IsTrue(File.Exists(SutRuleFile), $"{SutRuleFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            try
            {
                // Load rules
                Stream ruleStream = new MemoryStream(await FileFactoryService.GetInstance().ReadAllBytesAsync(SutRuleFile));
                SutRules = excelService.GetWorkbook(ruleStream);
                var matchingEntity = SutRules.ToMatchingRule();
                // Load data source
                Stream dataSourceStream = new MemoryStream(await FileFactoryService.GetInstance().ReadAllBytesAsync(SutDataSourceFile));
                SutWorkbook = excelService.GetWorkbook(dataSourceStream, Path.GetFileName(SutRuleFile));
                foreach (var sheet in SutWorkbook.Sheets)
                {
                    var dataSourceRecords = new List<DataSourceEntity>();
                    foreach (var row in sheet.Rows)
                        dataSourceRecords.Add(new DataSourceEntity(row));
                    var workflowLink = new LinkDataSourceSequentialActivity<DataSourceEntity>();
                    var linkResults = workflowLink.Execute(matchingEntity, dataSourceRecords);
                    Assert.IsTrue(linkResults.Any(), "No results from filter service.");
                }
            }
            catch (Exception ex)
            {
                logItem.LogError(ex.Message, ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task Link_HtmlScrapeSequential_Persist()
        {
            Assert.IsTrue(File.Exists(SutDataSourceFile), $"{SutDataSourceFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            Assert.IsTrue(File.Exists(SutRuleFile), $"{SutRuleFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            try
            {
                // Load rules
                Stream ruleStream = new MemoryStream(await FileFactoryService.GetInstance().ReadAllBytesAsync(SutRuleFile));
                SutRules = excelService.GetWorkbook(ruleStream);
                var matchingEntity = SutRules.ToMatchingRule();
                // Load data source
                Stream dataSourceStream = new MemoryStream(await FileFactoryService.GetInstance().ReadAllBytesAsync(SutDataSourceFile));
                SutWorkbook = excelService.GetWorkbook(dataSourceStream, Path.GetFileName(SutRuleFile));
                foreach (var sheet in SutWorkbook.Sheets)
                {
                    var dataSourceRecords = sheet.ToDataSourceEntity();
                    var workflowLink = new LinkDataSourceSequentialActivity<DataSourceEntity>();
                    var linkResults = workflowLink.Execute(matchingEntity, dataSourceRecords);
                    Assert.IsTrue(linkResults.Any(), "No results from filter service.");
                    var workflowPersist = new PersistMatchResultActivity<DataSourceEntity>(configDestination);
                    var persistResults = await workflowPersist.ExecuteAsync(linkResults);
                    Assert.IsTrue(persistResults.Any(), "No results from filter service.");
                }
            }
            catch (Exception ex)
            {
                logItem.LogError(ex.Message, ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task Link_HtmlScrapeSequential_Storage()
        {
            //await new Persist_DataSource_ActivityTests().Ingress_DataSource_Orchestration();
            //await new Persist_RulesSequential_ActivityTests().Ingress_RulesSequential_Orchestration();

            var configRulesLocal = new StorageTablesServiceConfiguration(
                "DefaultEndpointsProtocol=https;AccountName=stssaasdev001;AccountKey=+JxJop8Zfv5rk1JL9NaYcaVHPn60klyePxLQYv1Gsl2jHVbsanW+GukaVVU0i47+TKewxaUdN3HRdKrpcAhUcw==;EndpointSuffix=core.windows.net",
                StorageTableNames.RuleSequentialTable);
            var configDataSourceLocal = new StorageTablesServiceConfiguration(
                "DefaultEndpointsProtocol=https;AccountName=stssaasdev001;AccountKey=+JxJop8Zfv5rk1JL9NaYcaVHPn60klyePxLQYv1Gsl2jHVbsanW+GukaVVU0i47+TKewxaUdN3HRdKrpcAhUcw==;EndpointSuffix=core.windows.net",
                StorageTableNames.DataSourceTable);
            var configDestinationLocal = new StorageTablesServiceConfiguration(
                "DefaultEndpointsProtocol=https;AccountName=stssaasdev001;AccountKey=+JxJop8Zfv5rk1JL9NaYcaVHPn60klyePxLQYv1Gsl2jHVbsanW+GukaVVU0i47+TKewxaUdN3HRdKrpcAhUcw==;EndpointSuffix=core.windows.net",
                StorageTableNames.ResultsSequentialTable);

            var rules = new StorageTablesService<MatchingRuleEntity>(configRulesLocal).GetAndCastItems(r => r.PartitionKey != "");
            var dataSource = new StorageTablesService<DataSourceEntity>(configDataSourceLocal).GetAndCastItems(r => r.PartitionKey != "");
            var workflowLink = new LinkDataSourceSequentialActivity<DataSourceEntity>();
            var linkResults = workflowLink.Execute(rules, dataSource);
            var workflowPersist = new PersistMatchResultActivity<DataSourceEntity>(configDestinationLocal);

            var results = await workflowPersist.ExecuteAsync(linkResults);
            Assert.IsTrue(results.Any());
        }

        [TestCleanup]
        public void Cleanup()
        {
        }
    }
}

