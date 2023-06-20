namespace Detectify.Tests
{
    public class LambdaControllerTests
    {
        private readonly ILambdaController _lambdaController;
        private readonly IAwsConfiguration _awsConfiguration;

        public LambdaControllerTests()
        {
            _awsConfiguration = Initialization.AwsConfigurationFromEnvironmentVariables();
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
            Assert.NotNull(result);
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
            Assert.NotNull(result);
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
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, contentResult.StatusCode);
            Assert.Equal("text/plain", contentResult.ContentType);
        }
    }
}
