﻿using GoodToCode.Analytics.Abstractions;
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

namespace GoodToCode.Analytics.Matching.Tests
{
    [TestClass]
    public class Filter_SequentialFilter_StepTests
    {
        private readonly ILogger<Filter_SequentialFilter_StepTests> logItem;
        private readonly ExcelService excelService;
        private static string SutOpinionFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/OpinionFile.xlsx"; } }
        private static string SutDataSourceFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/03-Matching-DataSource-Small.xlsx"; } }
        private static string SutRuleFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/03-Matching-Rule-Sequential.xlsx"; } }
        public RowEntity SutRow { get; private set; }
        public IEnumerable<ICellData> SutCells { get; private set; }
        public IEnumerable<FilterExpression<ICellData>> SutFilters { get; private set; }
        public Dictionary<string, StringValues> SutReturn { get; private set; }

        public Filter_SequentialFilter_StepTests()
        {
            logItem = LoggerFactory.CreateLogger<Filter_SequentialFilter_StepTests>();
            excelService = ExcelServiceFactory.GetInstance().CreateExcelService();
        }

        [TestMethod]
        public async Task SequentialFilter_Activity_OpinionFile()
        {
            Assert.IsTrue(File.Exists(SutOpinionFile), $"{SutOpinionFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            SutFilters = new List<FilterExpression<ICellData>>() { new FilterExpression<ICellData>(x => x.ColumnIndex > -1) };

            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutOpinionFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                SutCells = excelService.GetSheet(itemToAnalyze, 0).Cells;
                var workflow = new SequentialFilterStep<ICellData>(SutFilters);
                var results = workflow.Execute(SutCells);
                Assert.IsTrue(results.Any(), "No results from filter service.");
                var nonBlanks = results.Where(x => string.IsNullOrWhiteSpace(x.CellValue) == false);
                Assert.IsTrue(nonBlanks.Any(), "No results from filter service.");
            }
            catch (Exception ex)
            {
                logItem.LogError(ex.Message, ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task SequentialFilter_Activity_ByAddressAndH2()
        {
            Assert.IsTrue(File.Exists(SutDataSourceFile), $"{SutDataSourceFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            Assert.IsTrue(File.Exists(SutRuleFile), $"{SutRuleFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            SutFilters = new List<FilterExpression<ICellData>>() {
                new FilterExpression<ICellData>(x => x.ColumnName == "Address" && x.CellValue.Contains("/bulk-discounts") == false),
                new FilterExpression<ICellData>(x => x.ColumnName == "H2-1" && x.CellValue == "Certification Discounts")
            };

            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutDataSourceFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                SutCells = excelService.GetSheet(itemToAnalyze, 0).Cells;
                var workflow = new SequentialFilterStep<ICellData>(SutFilters);
                var results = workflow.Execute(SutCells);
                Assert.IsTrue(results.Any(), "No results from filter service.");
                var nonBlanks = results.Where(x => string.IsNullOrWhiteSpace(x.CellValue) == false);
                Assert.IsTrue(nonBlanks.Any(), "No results from filter service.");
            }
            catch (Exception ex)
            {
                logItem.LogError(ex.Message, ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task SequentialFilter_Activity_ByAddressAndH1()
        {
            Assert.IsTrue(File.Exists(SutDataSourceFile), $"{SutDataSourceFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            Assert.IsTrue(File.Exists(SutRuleFile), $"{SutRuleFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            SutFilters = new List<FilterExpression<ICellData>>() { 
                new FilterExpression<ICellData>(x => x.ColumnName == "Address" && x.CellValue.Contains("/newsroom/clinical-voices") == false),
                new FilterExpression<ICellData>(x => x.ColumnName == "H1-1" && x.CellValue.StartsWith("Clinical Voices") == false)
            };

            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutDataSourceFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                SutCells = excelService.GetSheet(itemToAnalyze, 0).Cells;
                var workflow = new SequentialFilterStep<ICellData>(SutFilters);
                var results = workflow.Execute(SutCells);
                Assert.IsTrue(results.Any(), "No results from filter service.");
                var nonBlanks = results.Where(x => string.IsNullOrWhiteSpace(x.CellValue) == false);
                Assert.IsTrue(nonBlanks.Any(), "No results from filter service.");
            }
            catch (Exception ex)
            {
                logItem.LogError(ex.Message, ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task SequentialFilter_Activity_ByAddressAndTitle()
        {
            Assert.IsTrue(File.Exists(SutDataSourceFile), $"{SutDataSourceFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            Assert.IsTrue(File.Exists(SutRuleFile), $"{SutRuleFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            SutFilters = new List<FilterExpression<ICellData>>() {
                new FilterExpression<ICellData>(x => x.ColumnName == "Address" && x.CellValue.Contains("/education/webinar-series") == false),
                new FilterExpression<ICellData>(x => x.ColumnName == "H1-1" && x.CellValue.StartsWith("Nurse") == false)
            };

            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutDataSourceFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                SutCells = excelService.GetSheet(itemToAnalyze, 0).Cells;
                var workflow = new SequentialFilterStep<ICellData>(SutFilters);
                var results = workflow.Execute(SutCells);
                Assert.IsTrue(results.Any(), "No results from filter service.");
                var nonBlanks = results.Where(x => string.IsNullOrWhiteSpace(x.CellValue) == false);
                Assert.IsTrue(nonBlanks.Any(), "No results from filter service.");
            }
            catch (Exception ex)
            {
                logItem.LogError(ex.Message, ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task SequentialFilter_Activity_ByAddress()
        {
            Assert.IsTrue(File.Exists(SutDataSourceFile), $"{SutDataSourceFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            Assert.IsTrue(File.Exists(SutRuleFile), $"{SutRuleFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            SutFilters = new List<FilterExpression<ICellData>>() {
                new FilterExpression<ICellData>(x => x.ColumnName == "Address" && x.CellValue.Contains("/education/webinar-series") == false)
            };

            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutDataSourceFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                SutCells = excelService.GetSheet(itemToAnalyze, 0).Cells;
                var workflow = new SequentialFilterStep<ICellData>(SutFilters);
                var results = workflow.Execute(SutCells);
                Assert.IsTrue(results.Any(), "No results from filter service.");
                var nonBlanks = results.Where(x => string.IsNullOrWhiteSpace(x.CellValue) == false);
                Assert.IsTrue(nonBlanks.Any(), "No results from filter service.");
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

