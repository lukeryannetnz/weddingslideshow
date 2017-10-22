namespace weddingslideshow.api.Tests
{
    using System.IO;
    using System.Threading.Tasks;
    using System.Linq;

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

            Assert.Equal(200, response.StatusCode);
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
        }

        /// <summary>
        /// Tests that when a page is specified, 200 and data is returned.
        /// Flickr supports explicit paging where a page number is supplied.
        /// </summary>
        [Fact]
        public async Task TestGetPage200()
        {
            var lambdaFunction = new LambdaEntryPoint();

            var requestStr = File.ReadAllText("./SampleRequests/ImagesController-Get-Page.json");
            var request = JsonConvert.DeserializeObject<APIGatewayProxyRequest>(requestStr);
            var context = new TestLambdaContext();
            var response = await lambdaFunction.FunctionHandlerAsync(request, context);

            var responseBody = JsonConvert.DeserializeObject<PagedImageResponse>(response.Body);

            Assert.Equal(200, response.StatusCode);
            Assert.NotNull(responseBody);
            Assert.NotNull(responseBody.Data);
        }

        /// <summary>
        /// Tests that when a maxId is specified, a server error result is returned.
        /// Instagram used to support paging via an explicit id, so you page forward from the item id.
        /// For now leaving both params, the thinking behind this is that multiple or configurable data sources might be a goal in the future.
        /// </summary>
        [Fact]
        public async Task TestGetMaxIdServerError()
        {
            var lambdaFunction = new LambdaEntryPoint();

            var requestStr = File.ReadAllText("./SampleRequests/ImagesController-Get-MaxId.json");
            var request = JsonConvert.DeserializeObject<APIGatewayProxyRequest>(requestStr);
            var context = new TestLambdaContext();
            var response = await lambdaFunction.FunctionHandlerAsync(request, context);

            var responseBody = JsonConvert.DeserializeObject<PagedImageResponse>(response.Body);

            Assert.Equal(500, response.StatusCode);
        }
    }
}
