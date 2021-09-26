using GoodToCode.Analytics.Abstractions;
using GoodToCode.Analytics.Matching.Activities;
using GoodToCode.Analytics.Matching.Domain;
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
    public class Filter_SequentialFilter_ActivityTests
    {
        private readonly ILogger<Filter_SequentialFilter_ActivityTests> logItem;
        private string SutXlsxFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/OpinionFile.xlsx"; } }
        public RowEntity SutRow { get; private set; }
        public IEnumerable<RowEntity> SutRows { get; private set; }
        public IEnumerable<FilterExpression<RowEntity>> SutFilters { get; private set; }
        public Dictionary<string, StringValues> SutReturn { get; private set; }


        public Filter_SequentialFilter_ActivityTests()
        {
            logItem = LoggerFactory.CreateLogger<Filter_SequentialFilter_ActivityTests>();
        }

        [TestMethod]
        public async Task Column_Search_Activity()
        {
            Assert.IsTrue(File.Exists(SutXlsxFile), $"{SutXlsxFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            SutFilters = new List<FilterExpression<RowEntity>>() { new FilterExpression<RowEntity>(x => x.ColumnName.Contains("Answer")) };
            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutXlsxFile);
                Stream itemToAnalyze = new MemoryStream(bytes);
                var workflow = new SequentialFilterActivity<RowEntity>(SutFilters);
                var results = workflow.Execute(SutRows);
                Assert.IsTrue(results.Any(), "No results from filter service.");
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

