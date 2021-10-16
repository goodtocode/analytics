using GoodToCode.Analytics.Ingress.Activities;
using GoodToCode.Analytics.Ingress.Domain;
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

namespace GoodToCode.Analytics.Ingress.Unit.Tests
{
    [TestClass]
    public class Csv_Column_ActivityTests
    {
        private readonly ILogger<Csv_Column_ActivityTests> logItem;
        private static string SutCsvFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/OpinionFile.csv"; } }
        public RowEntity SutRow { get; private set; }
        public IEnumerable<RowEntity> SutRows { get; private set; }
        public Dictionary<string, StringValues> SutReturn { get; private set; }


        public Csv_Column_ActivityTests()
        {
            logItem = LoggerFactory.CreateLogger<Csv_Column_ActivityTests>();
        }

        [TestMethod]
        public async Task Column_Load_Activity()
        {
            Assert.IsTrue(File.Exists(SutCsvFile), $"{SutCsvFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutCsvFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                var workflow = new CsvColumnLoadActivity(new CsvService());
                var results = workflow.Execute(itemToAnalyze, 3);
                Assert.IsTrue(results.Any(), "No results from analytics service.");
            }
            catch (Exception ex)
            {
                logItem.LogError(ex.Message, ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task Column_Search_Activity()
        {
            Assert.IsTrue(File.Exists(SutCsvFile), $"{SutCsvFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutCsvFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                var workflow = new CsvColumnSearchActivity(new CsvService());
                var results = workflow.Execute(itemToAnalyze, "DocName", "*");
                Assert.IsTrue(results.Any(), "No results from analytics service.");
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

