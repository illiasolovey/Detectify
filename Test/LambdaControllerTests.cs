using Xunit;
using Moq;
using Detectify.Controllers;
using Detectify.Models;
using Microsoft.AspNetCore.Mvc;

namespace Detectify.Tests
{
    public class LambdaControllerTests
    {
        private readonly ILambdaController _lambdaController;
        private Mock<IAwsConfiguration> _awsConfigurationMock;

        public LambdaControllerTests()
        {
            _awsConfigurationMock = InitializeConfigurationMock();
            _lambdaController = new LambdaController(_awsConfigurationMock.Object);
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

        private Mock<IAwsConfiguration> InitializeConfigurationMock()
        {
            Mock<IAwsConfiguration> awsConfig = new Mock<IAwsConfiguration>();
            awsConfig.Setup(x => x.AccessKey).Returns(Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID")!);
            awsConfig.Setup(x => x.SecretKey).Returns(Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY")!);
            awsConfig.Setup(x => x.Region).Returns(Environment.GetEnvironmentVariable("AWS_REGION")!);
            var lambdaFunctions = new Dictionary<string, string>
            {
                { "ObjectAnalysis", Environment.GetEnvironmentVariable("AWS_LAMBDA_OBJECT_ANALYSIS")! },
                { "Celebrity", Environment.GetEnvironmentVariable("AWS_LAMBDA_CELEBRITY_RECOGNITION")! }
            };
            awsConfig.Setup(x => x.LambdaFunctions).Returns(lambdaFunctions);
            return awsConfig;
        }
    }
}
