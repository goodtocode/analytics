﻿using GoodToCode.Analytics.Abstractions;
using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Blob.Excel;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.Matching.Tests
{
    [TestClass]
    public class Matching_MatchingRuleEntity_ExtensionsTests
    {
        private readonly ILogger<Matching_MatchingRuleEntity_ExtensionsTests> logItem;
        private readonly ExcelService excelService;
        private static string SutRuleFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/03-Matching-Rule-Sequential.xlsx"; } }
        public IWorkbookData SutWorkbook { get; private set; }
        public ISheetData SutSheet { get; private set; }
        public FilterExpression<ICellData> SutFilter { get; private set; }


        public Matching_MatchingRuleEntity_ExtensionsTests()
        {
            logItem = LoggerFactory.CreateLogger<Matching_MatchingRuleEntity_ExtensionsTests>();
            excelService = ExcelServiceFactory.GetInstance().CreateExcelService();
        }

        [TestMethod]
        public async Task Matching_MatchingRuleEntity_ToFilterExpression()
        {
            Assert.IsTrue(File.Exists(SutRuleFile), $"{SutRuleFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutRuleFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                SutSheet = excelService.GetSheet(itemToAnalyze, 0);
                var matchingEntity = SutSheet.ToMatchingRule();
                Assert.IsTrue(matchingEntity.Any());
                Assert.IsTrue(matchingEntity.FirstOrDefault().MatchColumn != null);
                var filter = matchingEntity.FirstOrDefault().ToFilterExpression<DataSourceEntity>();
                Assert.IsTrue(filter.Expression != null);
            }
            catch (Exception ex)
            {
                logItem.LogError(ex.Message, ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task Matching_WorkbookData_ToMatchingRule()
        {
            Assert.IsTrue(File.Exists(SutRuleFile), $"{SutRuleFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutRuleFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                SutWorkbook = excelService.GetWorkbook(itemToAnalyze);
                var matchingEntity = SutWorkbook.ToMatchingRule();
                Assert.IsTrue(matchingEntity.Any());
                Assert.IsTrue(matchingEntity.FirstOrDefault().MatchColumn != null);
            }
            catch (Exception ex)
            {
                logItem.LogError(ex.Message, ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task Matching_SheetData_ToMatchingRule()
        {
            Assert.IsTrue(File.Exists(SutRuleFile), $"{SutRuleFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutRuleFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                SutSheet = excelService.GetSheet(itemToAnalyze, 0);
                var matchingEntity = SutSheet.ToMatchingRule();
                Assert.IsTrue(matchingEntity.Any());
                Assert.IsTrue(matchingEntity.FirstOrDefault().MatchColumn != null);
            }
            catch (Exception ex)
            {
                logItem.LogError(ex.Message, ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task Matching_WorkbookData_Order()
        {
            Assert.IsTrue(File.Exists(SutRuleFile), $"{SutRuleFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutRuleFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                SutWorkbook = excelService.GetWorkbook(itemToAnalyze);
                var matchingEntity = SutWorkbook.ToMatchingRule();
                Assert.IsTrue(matchingEntity.Any());
                Assert.IsTrue(matchingEntity.FirstOrDefault().MatchColumn != null);
                Assert.IsTrue(matchingEntity.FirstOrDefault().Order < matchingEntity.LastOrDefault().Order);
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

