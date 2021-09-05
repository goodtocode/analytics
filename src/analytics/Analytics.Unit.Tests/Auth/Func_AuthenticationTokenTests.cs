//using Microsoft.AspNetCore.Http;
//using Microsoft.Azure.Functions.Worker.Http;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Threading.Tasks;

//namespace GoodToCode.Analytics
//{
//    [TestClass]
//    public class Func_AuthenticationTokenTests
//    {
//        private readonly ILogger<Healthcheck> logger = LoggerFactory.CreateLogger<Healthcheck>();
//        private readonly IConfiguration config = new AppConfigurationFactory().Create();

//        public string Sut { get; private set; }
//        public bool SutResponse { get; private set; }

//        public Func_AuthenticationTokenTests()
//        {            
//        }

//        [TestMethod]
//        public async Task AuthToken_Verify()
//        {
//            //Sut = $"{config["Reflections:Shared:FunctionsUrl"]}/api/Healthcheck?code={config["Reflections:Shared:FunctionsCode"]}";
//            //Assert.IsTrue(Sut.Length > 0);
//            //var request = new HttpRequestFactory("GET").CreateHttpRequest("code", config["Reflections:Shared:FunctionsCode"]);
//            //var response = await new Healthcheck(logger, config).Run(request, null);
//            //var returnedItem = (FunctionHealth)response.Value;
//            //SutResponse = returnedItem.Connected;
//            //Assert.IsTrue(response.StatusCode == StatusCodes.Status200OK);
//            //Assert.IsTrue(SutResponse);
//        }
//    }
//}
