using GoodToCode.Analytics.Abstractions;
using GoodToCode.Analytics.Matching.Activities;
using GoodToCode.Analytics.Matching.Domain;
using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Blob.Excel;
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
    public class Matching_MatchingRuleEntity_ExtensionsTests
    {
        private readonly ILogger<Matching_MatchingRuleEntity_ExtensionsTests> logItem;
        private readonly ExcelService excelService;
        private static string SutOpinionFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/OpinionFile.xlsx"; } }
        private static string SutDataSourceFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/Matching-DataSource-Small.xlsx"; } }
        private static string SutRuleFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/Matching-Rule-Sequential.xlsx"; } }
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

