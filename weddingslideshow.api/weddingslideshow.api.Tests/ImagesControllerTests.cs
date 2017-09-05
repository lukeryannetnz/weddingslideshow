namespace weddingslideshow.api.Tests
{
    using System.IO;
    using System.Threading.Tasks;

    using Amazon.Lambda.APIGatewayEvents;
    using Amazon.Lambda.TestUtilities;

    using Newtonsoft.Json;

    using Xunit;

    public class ImagesControllerTests
    {
        [Fact]
        public async Task TestGet()
        {
            var lambdaFunction = new LambdaEntryPoint();

            var requestStr = File.ReadAllText("./SampleRequests/ImagesController-Get.json");
            var request = JsonConvert.DeserializeObject<APIGatewayProxyRequest>(requestStr);
            var context = new TestLambdaContext();
            var response = await lambdaFunction.FunctionHandlerAsync(request, context);

            Assert.Equal(response.StatusCode, 200);
            Assert.Equal("[\"http://www.rewildthyself.com/wp-content/uploads/2015/03/poop.jpeg\"]", response.Body);
            Assert.True(response.Headers.ContainsKey("Content-Type"));
            Assert.Equal("application/json; charset=utf-8", response.Headers["Content-Type"]);
        }
    }
}
