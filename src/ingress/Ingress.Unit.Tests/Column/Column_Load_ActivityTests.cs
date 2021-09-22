using GoodToCode.Analytics.Ingress.Activities;
using GoodToCode.Analytics.Ingress.Domain;
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

namespace GoodToCode.Analytics.Ingress.Unit.Tests
{
    [TestClass]
    public class Column_Load_ActivityTests
    {
        private readonly ILogger<Column_Load_ActivityTests> logItem;
        private string SutXlsxFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/OpinionFile.xlsx"; } }
        public RowEntity SutRow { get; private set; }
        public IEnumerable<RowEntity> SutRows { get; private set; }
        public Dictionary<string, StringValues> SutReturn { get; private set; }


        public Column_Load_ActivityTests()
        {
            logItem = LoggerFactory.CreateLogger<Column_Load_ActivityTests>();
        }

        [TestMethod]
        public async Task Column_Load_Activity()       
        {
            Assert.IsTrue(File.Exists(SutXlsxFile), $"{SutXlsxFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            try
            { 
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutXlsxFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                var workflow = new  ExcelColumnSearchActivity(new ExcelService());
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

