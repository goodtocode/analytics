﻿using GoodToCode.Analytics.Ingress.Activities;
using GoodToCode.Analytics.Abstractions;
using GoodToCode.Shared.Blob.Csv;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.Ingress.Tests
{
    [TestClass]
    public class Csv_Sheet_ActivityTests
    {
        private readonly ILogger<Csv_Sheet_ActivityTests> logItem;
        private static string SutCsvFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/OpinionFile.csv"; } }
        public CellEntity SutRow { get; private set; }
        public IEnumerable<CellEntity> SutRows { get; private set; }
        public Dictionary<string, StringValues> SutReturn { get; private set; }


        public Csv_Sheet_ActivityTests()
        {
            logItem = LoggerFactory.CreateLogger<Csv_Sheet_ActivityTests>();
        }

        [TestMethod]
        public async Task Csv_Sheet_Load_Activity()       
        {
            Assert.IsTrue(File.Exists(SutCsvFile), $"{SutCsvFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            try
            { 
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutCsvFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                var workflow = new  CsvSheetLoadActivity(new CsvService());
                var results = workflow.Execute(itemToAnalyze);
                Assert.IsTrue(results.Rows.Any(), "No results from Csv service.");
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

