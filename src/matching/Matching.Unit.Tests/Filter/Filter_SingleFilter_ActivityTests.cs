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
    public class Filter_SingleFilter_ActivityTests
    {
        private readonly ILogger<Filter_SingleFilter_ActivityTests> logItem;
        private readonly ExcelService excelService;
        private string SutOpinionFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/OpinionFile.xlsx"; } }
        private string SutDataSourceFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/Matching-DataSource-Small.xlsx"; } }
        private string SutRuleFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/Matching-Rule-Sequential.xlsx"; } }
        public RowEntity SutRow { get; private set; }
        public IEnumerable<ICellData> SutHeaders { get; private set; }
        public FilterExpression<ICellData> SutFilter { get; private set; }
        public Dictionary<string, StringValues> SutReturn { get; private set; }


        public Filter_SingleFilter_ActivityTests()
        {
            logItem = LoggerFactory.CreateLogger<Filter_SingleFilter_ActivityTests>();
            excelService = ExcelServiceFactory.GetInstance().CreateExcelService();
        }

        [TestMethod]
        public async Task SingleFilter_Activity_OpinionFile()
        {
            Assert.IsTrue(File.Exists(SutOpinionFile), $"{SutOpinionFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            SutFilter = new FilterExpression<ICellData>(x => x.ColumnIndex > -1);

            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutOpinionFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                SutHeaders = excelService.GetSheet(itemToAnalyze, 0).Cells;
                var workflow = new SingleFilterActivity<ICellData>(SutFilter);
                var results = workflow.Execute(SutHeaders);
                Assert.IsTrue(results.Any(), "No results from filter service.");
                Assert.IsTrue(!string.IsNullOrWhiteSpace(results.FirstOrDefault().FirstOrDefault()?.CellValue), "No results from filter service.");
            }
            catch (Exception ex)
            {
                logItem.LogError(ex.Message, ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task SingleFilter_Activity_Invalid()
        {
            Assert.IsTrue(File.Exists(SutDataSourceFile), $"{SutDataSourceFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            Assert.IsTrue(File.Exists(SutRuleFile), $"{SutRuleFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            SutFilter = new FilterExpression<ICellData>(x => x.ColumnName == "Status Code" && x.CellValue != "OK"
                    && x.ColumnName == "Content Type" && x.CellValue != "text/html; charset=utf-8");

            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutDataSourceFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                SutHeaders = excelService.GetSheet(itemToAnalyze, 0).Cells;
                var workflow = new SingleFilterActivity<ICellData>(SutFilter);
                var results = workflow.Execute(SutHeaders);
                Assert.IsTrue(results.Any(), "No results from filter service.");
                Assert.IsTrue(!string.IsNullOrWhiteSpace(results.FirstOrDefault().FirstOrDefault()?.CellValue), "No results from filter service.");
            }
            catch (Exception ex)
            {
                logItem.LogError(ex.Message, ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task SingleFilter_Activity_ByAddressAndH2()
        {
            Assert.IsTrue(File.Exists(SutDataSourceFile), $"{SutDataSourceFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            Assert.IsTrue(File.Exists(SutRuleFile), $"{SutRuleFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            SutFilter = new FilterExpression<ICellData>(x => (x.ColumnName == "Address" && x.CellValue.Contains("/bulk-discounts"))
                    && (x.ColumnName == "H2-1" && x.CellValue == "Certification Discounts"));

            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutDataSourceFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                SutHeaders = excelService.GetSheet(itemToAnalyze, 0).Cells;
                var workflow = new SingleFilterActivity<ICellData>(SutFilter);
                var results = workflow.Execute(SutHeaders);
                Assert.IsTrue(results.Any(), "No results from filter service.");
                Assert.IsTrue(!string.IsNullOrWhiteSpace(results.FirstOrDefault().FirstOrDefault()?.CellValue), "No results from filter service.");
            }
            catch (Exception ex)
            {
                logItem.LogError(ex.Message, ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task SingleFilter_Activity_ByAddressAndH1()
        {
            Assert.IsTrue(File.Exists(SutDataSourceFile), $"{SutDataSourceFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            Assert.IsTrue(File.Exists(SutRuleFile), $"{SutRuleFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            SutFilter = new FilterExpression<ICellData>(x => (x.ColumnName == "Address" && x.CellValue.Contains("/newsroom/clinical-voices"))
                    && (x.ColumnName == "H1-1" && x.CellValue.StartsWith("Clinical Voices")));

            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutDataSourceFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                SutHeaders = excelService.GetSheet(itemToAnalyze, 0).Cells;
                var workflow = new SingleFilterActivity<ICellData>(SutFilter);
                var results = workflow.Execute(SutHeaders);
                Assert.IsTrue(results.Any(), "No results from filter service.");
                Assert.IsTrue(!string.IsNullOrWhiteSpace(results.FirstOrDefault().FirstOrDefault()?.CellValue), "No results from filter service.");
            }
            catch (Exception ex)
            {
                logItem.LogError(ex.Message, ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task SingleFilter_Activity_ByAddressAndTitle()
        {
            Assert.IsTrue(File.Exists(SutDataSourceFile), $"{SutDataSourceFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            Assert.IsTrue(File.Exists(SutRuleFile), $"{SutRuleFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            SutFilter = new FilterExpression<ICellData>(
                    x => (x.ColumnName == "Address" && x.CellValue.Contains("/education/webinar-series"))
                    && (x.ColumnName == "Title-1" && x.CellValue.StartsWith("Nurse Strong: Recognizing and Mitigating Moral Distress ")));

            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutDataSourceFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                SutHeaders = excelService.GetSheet(itemToAnalyze, 0).Cells;
                var workflow = new SingleFilterActivity<ICellData>(SutFilter);
                var results = workflow.Execute(SutHeaders);
                Assert.IsTrue(results.Any(), "No results from filter service.");
                Assert.IsTrue(!string.IsNullOrWhiteSpace(results.FirstOrDefault().FirstOrDefault()?.CellValue), "No results from filter service.");
            }
            catch (Exception ex)
            {
                logItem.LogError(ex.Message, ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task SingleFilter_Activity_ByAddress()
        {
            Assert.IsTrue(File.Exists(SutDataSourceFile), $"{SutDataSourceFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            Assert.IsTrue(File.Exists(SutRuleFile), $"{SutRuleFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            SutFilter = new FilterExpression<ICellData>(x => x.ColumnName == "Address" && x.CellValue != "/nursing-excellence/nurse-stories");

            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutDataSourceFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                SutHeaders = excelService.GetSheet(itemToAnalyze, 0).Cells;
                var workflow = new SingleFilterActivity<ICellData>(SutFilter);
                var results = workflow.Execute(SutHeaders);
                Assert.IsTrue(results.Any(), "No results from filter service.");
                Assert.IsTrue(!string.IsNullOrWhiteSpace(results.FirstOrDefault().FirstOrDefault()?.CellValue), "No results from filter service.");
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

