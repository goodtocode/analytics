using Azure.Data.Tables;
using GoodToCode.Analytics.CognitiveServices.Activities;
using GoodToCode.Analytics.CognitiveServices.Domain;
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

namespace GoodToCode.Analytics.CognitiveServices.Unit.Tests
{
    [TestClass]
    public class RowEntity_Persist_CloudTests
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<RowEntity_Persist_CloudTests> log;
        private readonly StorageTablesServiceConfiguration configStorage;
        private NpoiService serviceExcel;
        private string SutXlsxFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/OpinionFile.xlsx"; } }
        public RowEntity SutRow { get; private set; }
        public IEnumerable<RowEntity> SutRows { get; private set; }
        public List<TableEntity> SutReturn { get; private set; } = new List<TableEntity>();


        public RowEntity_Persist_CloudTests()
        {
            log = LoggerFactory.CreateLogger<RowEntity_Persist_CloudTests>();
            configuration = new AppConfigurationFactory().Create();
            configStorage = new StorageTablesServiceConfiguration(
                configuration[AppConfigurationKeys.StorageTablesConnectionString],
                $"UnitTest-{DateTime.UtcNow:yyyy-MM-dd}-RowEntity");

            serviceExcel = new NpoiService();
        }

        //[TestMethod]
        public async Task RowEntity_Persist_Cloud()
        {
            Assert.IsTrue(File.Exists(SutXlsxFile), $"{SutXlsxFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            try
            {
                var bytes = await FileFactoryService.GetInstance().ReadAllBytesAsync(SutXlsxFile);
                Stream itemToAnalyze = new MemoryStream(bytes);

                var sheet = serviceExcel.GetWorkbook(itemToAnalyze).GetSheetAt(0);
                var sd = sheet.ToSheetData();
                foreach (var row in sd.Rows)
                {
                    SutReturn.Add(await new RowEntityPersistActivity(configStorage).ExecuteAsync(new RowEntity(row.Cells.FirstOrDefault())));
                }    
                Assert.IsTrue(SutReturn.Any(), "No results from service.");
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
        }
    }
}

