using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Detectify.Tests
{
    public class LambdaControllerTests
    {
        private readonly ILambdaController _lambdaController;
        private readonly IAwsConfiguration _awsConfiguration;

        public LambdaControllerTests()
        {
            _awsConfiguration = new AwsConfiguration(
                accessKey: Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID")!,
                secretKey: Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY")!,
                region: Environment.GetEnvironmentVariable("AWS_REGION")!,
                lambdaFunctions: new Dictionary<string, string>
                {
                    { "ObjectAnalysis", Environment.GetEnvironmentVariable("AWS_LAMBDA_OBJECT_ANALYSIS")! },
                    { "Celebrity", Environment.GetEnvironmentVariable("AWS_LAMBDA_CELEBRITY_RECOGNITION")! }
                },
                s3Buckets: new Dictionary<string, string>
                {
                    { "Put", Environment.GetEnvironmentVariable("AWS_S3_INPUT_BUCKET")! },
                    { "Get", Environment.GetEnvironmentVariable("AWS_S3_OUTPUT_BUCKET")! }
                }
            );
            _lambdaController = new LambdaController(_awsConfiguration);
        }

        [Fact]
        public async Task InvokeObjectAnalysis_ValidParameters_ReturnsOkResult()
        {
            string filename = "testframe.jpg";
            float confidencePercentage = 80;
            string boundingBoxHex = "#FF0000";
            string labelHex = "#FFFFFF";

            var result = await _lambdaController.InvokeObjectAnalysis(filename, confidencePercentage, boundingBoxHex, labelHex);

            var contentResult = Assert.IsType<ContentResult>(result);
            Assert.Equal(StatusCodes.Status200OK, contentResult.StatusCode);
            Assert.Equal("text/plain", contentResult.ContentType);
        }

        [Fact]
        public async Task InvokeObjectAnalysis_MissingRequiredParameter_ReturnsBadRequest()
        {
            string filename = null!;
            float confidencePercentage = 80;
            string boundingBoxHex = "#FF0000";
            string labelHex = "#FFFFFF";

            var result = await _lambdaController.InvokeObjectAnalysis(filename, confidencePercentage, boundingBoxHex, labelHex);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task InvokeObjectAnalysis_MissingNonRequiredParameter_ReturnsOkResult()
        {
            string filename = "testframe.jpg";
            float confidencePercentage = 80;
            string boundingBoxHex = null!;
            string labelHex = "#FFFFFF";

            var result = await _lambdaController.InvokeObjectAnalysis(filename, confidencePercentage, boundingBoxHex, labelHex);

            var contentResult = Assert.IsType<ContentResult>(result);
            Assert.Equal(StatusCodes.Status200OK, contentResult.StatusCode);
            Assert.Equal("text/plain", contentResult.ContentType);
        }
    }
}
