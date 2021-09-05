//using GoodToCode.Shared.Analytics.Abstractions;
//using GoodToCode.Shared.Persistence.StorageTables;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Primitives;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Net.Http;
//using System.Reflection;
//using System.Threading.Tasks;

//namespace GoodToCode.Analytics.Unit.Tests
//{
//    [TestClass]
//    public class Func_Health_HealthcheckTests
//    {
//        private readonly ILogger<Func_Health_HealthcheckTests> log;
//        private readonly IConfiguration configuration;
//        private readonly string functionsUrl;

//        public Uri SutEndpoint { get; private set; }
//        public Dictionary<string, StringValues> SutReturn { get; private set; }


//        public Func_Health_HealthcheckTests()
//        {
//            log = LoggerFactory.CreateLogger<Func_Health_HealthcheckTests>();
//            configuration = new AppConfigurationFactory().Create();
//            functionsUrl = $"{configuration["GoodToCode:Analytics:FileToPersistence:FunctionsUrl"]}";
//            SutEndpoint = new Uri($"{functionsUrl}/api/healthcheck?code={configuration["GoodToCode:Analytics:FileToPersistence:FunctionsCode"]}");
//        }

//        [TestMethod]
//        public async Task Func_Health_Healthcheck()
//        {
//            try
//            {
//                var client = new HttpClient();
//                var response = await client.GetAsync(SutEndpoint);
//                Assert.IsTrue(response.IsSuccessStatusCode);
//            }
//            catch (Exception ex)
//            {
//                log.LogError(ex.Message, ex);
//                Assert.Fail(ex.Message);
//            }
//        }
//    }
//}

