using Detectify.Filters;
using Detectify.Services;
using Microsoft.Extensions.Logging;

namespace Detectify.Tests
{
    public class S3ControllerTests
    {
        private readonly Mock<IStorageService> _storageServiceMock;
        private readonly Mock<ILogger<S3Controller>> _loggerMock;
        private readonly S3Controller _s3Controller;
        private string _testFileObjectKey = null!;

        public S3ControllerTests()
        {
            var awsConfiguration = Initialization.AwsConfigurationFromEnvironmentVariables();
            _loggerMock = new Mock<ILogger<S3Controller>>();
            _storageServiceMock = new Mock<IStorageService>();
            _s3Controller = new S3Controller(_storageServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task UploadObject_ValidInput_ReturnsOkResult()
        {
            var formFileMock = new Mock<IFormFile>();
            formFileMock.Setup(file => file.Length).Returns(3);

            var result = await _s3Controller.UploadObject("testFile.jpg", formFileMock.Object) as OkObjectResult;

            Assert.NotNull(result);
            _testFileObjectKey = result.ToString() ?? throw new Exception("Failed to receive necessary response body.");
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async Task UploadObject_EmptyInputFile_ReturnsBadRequestResult()
        {
            var formFileMock = new Mock<IFormFile>();
            formFileMock.Setup(f => f.Length).Returns(0);
            _storageServiceMock.Setup(s => s.UploadObjectAsync(It.IsAny<S3Object>())).Throws(new Exception("Test"));

            var result = await _s3Controller.UploadObject("testFile.jpg", formFileMock.Object) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.Equal("Incoming file contains no data.", result.Value);
        }

        [Fact]
        public async Task UploadObject_Exception_ReturnsBadRequestResult()
        {
            var formFileMock = new Mock<IFormFile>();
            formFileMock.Setup(f => f.Length).Returns(3);
            _storageServiceMock.Setup(s => s.UploadObjectAsync(It.IsAny<S3Object>())).Throws(new Exception());

            var result = await _s3Controller.UploadObject("testFile.jpg", formFileMock.Object) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task DownloadObject_ValidInput_ReturnsFileResult()
        {
            _storageServiceMock.Setup(s => s.DownloadObjectAsync(_testFileObjectKey))
                .ReturnsAsync(new byte[0]);

            var result = await _s3Controller.DownloadObject("sample.jpg");

            Assert.NotNull(result);
            Assert.IsType<FileContentResult>(result);
        }

        [Fact]
        public async Task DownloadObject_FileNotFound_ReturnsNotFoundResult()
        {
            _storageServiceMock.Setup(s => s.DownloadObjectAsync(It.IsAny<string>()))
                .Throws(new FileNotFoundException());

            var result = await _s3Controller.DownloadObject("sample.jpg") as NotFoundObjectResult;

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [Fact]
        public async Task DownloadObject_Exception_ReturnsBadRequestResult()
        {
            _storageServiceMock.Setup(s => s.DownloadObjectAsync(It.IsAny<string>()))
                .Throws(new Exception());

            var result = await _s3Controller.DownloadObject("sample.jpg") as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }
    }
}