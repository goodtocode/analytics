using GoodToCode.Analytics.Abstractions;
using GoodToCode.Analytics.Matching.Activities;
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
    public class Filter_MultiFilter_ActivityTests
    {
        private readonly ILogger<Filter_MultiFilter_ActivityTests> logItem;
        private readonly ExcelService excelService;
        private static string SutOpinionFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/OpinionFile.xlsx"; } }
        private static string SutDataSourceFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/Matching-DataSource-Small.xlsx"; } }
        private static string SutRuleFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/Matching-Rule-Sequential-Small.xlsx"; } }
        public RowEntity SutRow { get; private set; }
        public IEnumerable<ICellData> SutSheet { get; private set; }
        public IEnumerable<FilterExpression<ICellData>> SutFilters { get; private set; }
        public Dictionary<string, StringValues> SutReturn { get; private set; }


        public Filter_MultiFilter_ActivityTests()
        {
            logItem = LoggerFactory.CreateLogger<Filter_MultiFilter_ActivityTests>();
            excelService = ExcelServiceFactory.GetInstance().CreateExcelService();
        }

        [TestMethod]
        public async Task MultiFilter_Activity_OpinionFile()
        {
            Assert.IsTrue(File.Exists(SutOpinionFile), $"{SutOpinionFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            SutFilters = new List<FilterExpression<ICellData>>() { 
                new FilterExpression<ICellData>(x => x.ColumnIndex > -1) };

            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutOpinionFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                SutSheet = excelService.GetSheet(itemToAnalyze, 0).Cells;
                var workflow = new MultiFilterActivity<ICellData>(SutFilters);
                var results = workflow.Execute(SutSheet);
                Assert.IsTrue(results.Any(), "No results from filter service.");
                Assert.IsTrue(!string.IsNullOrWhiteSpace(results.FirstOrDefault()?.CellValue), "No results from filter service.");
            }
            catch (Exception ex)
            {
                logItem.LogError(ex.Message, ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task MultiFilter_Activity_Invalid()
        {
            Assert.IsTrue(File.Exists(SutDataSourceFile), $"{SutDataSourceFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            Assert.IsTrue(File.Exists(SutRuleFile), $"{SutRuleFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            SutFilters = new List<FilterExpression<ICellData>>() {
                new  FilterExpression<ICellData>(x => x.ColumnName == "Status" && x.CellValue != "OK") };

            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutDataSourceFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                SutSheet = excelService.GetSheet(itemToAnalyze, 0).Cells;
                var workflow = new MultiFilterActivity<ICellData>(SutFilters);
                var results = workflow.Execute(SutSheet);
                Assert.IsTrue(results.Any(), "No results from filter service.");
                Assert.IsTrue(!string.IsNullOrWhiteSpace(results.FirstOrDefault()?.CellValue), "No results from filter service.");
            }
            catch (Exception ex)
            {
                logItem.LogError(ex.Message, ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task MultiFilter_Activity_ByH2()
        {
            Assert.IsTrue(File.Exists(SutDataSourceFile), $"{SutDataSourceFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            Assert.IsTrue(File.Exists(SutRuleFile), $"{SutRuleFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            SutFilters = new List<FilterExpression<ICellData>>() {
                new  FilterExpression<ICellData>(x => x.ColumnName == "Address" && x.CellValue.Contains("/bulk-discounts")) };

            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutDataSourceFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                SutSheet = excelService.GetSheet(itemToAnalyze, 0).Cells;
                var workflow = new MultiFilterActivity<ICellData>(SutFilters);
                var results = workflow.Execute(SutSheet);
                Assert.IsTrue(results.Any(), "No results from filter service.");
                Assert.IsTrue(!string.IsNullOrWhiteSpace(results.FirstOrDefault()?.CellValue), "No results from filter service.");
            }
            catch (Exception ex)
            {
                logItem.LogError(ex.Message, ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task MultiFilter_Activity_ByH1()
        {
            Assert.IsTrue(File.Exists(SutDataSourceFile), $"{SutDataSourceFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            Assert.IsTrue(File.Exists(SutRuleFile), $"{SutRuleFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            SutFilters = new List<FilterExpression<ICellData>>() {
                new  FilterExpression<ICellData>(x => x.ColumnName == "H1-1" && x.CellValue.StartsWith("Clinical Voices")) };

            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutDataSourceFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                SutSheet = excelService.GetSheet(itemToAnalyze, 0).Cells;
                var workflow = new MultiFilterActivity<ICellData>(SutFilters);
                var results = workflow.Execute(SutSheet);
                Assert.IsTrue(results.Any(), "No results from filter service.");
                Assert.IsTrue(!string.IsNullOrWhiteSpace(results.FirstOrDefault()?.CellValue), "No results from filter service.");
            }
            catch (Exception ex)
            {
                logItem.LogError(ex.Message, ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task MultiFilter_Activity_ByTitle()
        {
            Assert.IsTrue(File.Exists(SutDataSourceFile), $"{SutDataSourceFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            Assert.IsTrue(File.Exists(SutRuleFile), $"{SutRuleFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            SutFilters = new List<FilterExpression<ICellData>>() {
                new  FilterExpression<ICellData>(
                    x => x.ColumnName == "Title 1" && x.CellValue.StartsWith("Nurse Strong")) };

            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutDataSourceFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                SutSheet = excelService.GetSheet(itemToAnalyze, 0).Cells;
                var workflow = new MultiFilterActivity<ICellData>(SutFilters);
                var results = workflow.Execute(SutSheet);
                Assert.IsTrue(results.Any(), "No results from filter service.");
                Assert.IsTrue(!string.IsNullOrWhiteSpace(results.FirstOrDefault()?.CellValue), "No results from filter service.");
            }
            catch (Exception ex)
            {
                logItem.LogError(ex.Message, ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task MultiFilter_Activity_ByAddress()
        {
            Assert.IsTrue(File.Exists(SutDataSourceFile), $"{SutDataSourceFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            Assert.IsTrue(File.Exists(SutRuleFile), $"{SutRuleFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            SutFilters = new List<FilterExpression<ICellData>>() {
                new  FilterExpression<ICellData>(x => x.ColumnName == "Address" && x.CellValue != "/nursing-excellence/nurse-stories"),
                new  FilterExpression<ICellData>(x => x.ColumnName == "Address" && x.CellValue != "/nursing-excellence/chapters")};

            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutDataSourceFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                SutSheet = excelService.GetSheet(itemToAnalyze, 0).Cells;
                var workflow = new MultiFilterActivity<ICellData>(SutFilters);
                var results = workflow.Execute(SutSheet);
                Assert.IsTrue(results.Any(), "No results from filter service.");
                Assert.IsTrue(!string.IsNullOrWhiteSpace(results.FirstOrDefault()?.CellValue), "No results from filter service.");
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

