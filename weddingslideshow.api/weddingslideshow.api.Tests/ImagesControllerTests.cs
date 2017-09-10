namespace weddingslideshow.api.Tests
{
    using System.IO;
    using System.Threading.Tasks;

    using Amazon.Lambda.APIGatewayEvents;
    using Amazon.Lambda.TestUtilities;
    using Newtonsoft.Json;
    using Xunit;

    using weddingslideshow.api.Contracts;

    public class ImagesControllerTests
    {
        [Fact]
        public async Task TestGet200Json()
        {
            var lambdaFunction = new LambdaEntryPoint();

            var requestStr = File.ReadAllText("./SampleRequests/ImagesController-Get.json");
            var request = JsonConvert.DeserializeObject<APIGatewayProxyRequest>(requestStr);
            var context = new TestLambdaContext();
            var response = await lambdaFunction.FunctionHandlerAsync(request, context);

            Assert.Equal(response.StatusCode, 200);
            Assert.True(response.Headers.ContainsKey("Content-Type"));
            Assert.Equal("application/json; charset=utf-8", response.Headers["Content-Type"]);
        }

        [Fact]
        public async Task TestGetPagination()
        {
            var lambdaFunction = new LambdaEntryPoint();

            var requestStr = File.ReadAllText("./SampleRequests/ImagesController-Get.json");
            var request = JsonConvert.DeserializeObject<APIGatewayProxyRequest>(requestStr);
            var context = new TestLambdaContext();
            var response = await lambdaFunction.FunctionHandlerAsync(request, context);

            var responseBody = JsonConvert.DeserializeObject<PagedImageResponse>(response.Body);

            Assert.NotNull(responseBody);
            Assert.NotNull(responseBody.Pagination);
        }

        [Fact]
        public async Task TestGetData()
        {
            var lambdaFunction = new LambdaEntryPoint();

            var requestStr = File.ReadAllText("./SampleRequests/ImagesController-Get.json");
            var request = JsonConvert.DeserializeObject<APIGatewayProxyRequest>(requestStr);
            var context = new TestLambdaContext();
            var response = await lambdaFunction.FunctionHandlerAsync(request, context);

            var responseBody = JsonConvert.DeserializeObject<PagedImageResponse>(response.Body);

            Assert.NotNull(responseBody);
            Assert.NotNull(responseBody.Data);
            Assert.Equal("1", responseBody.Data[0].Id)
        }
    }
}
