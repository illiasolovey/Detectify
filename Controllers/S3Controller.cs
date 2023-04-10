using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Detectify.Models;
using Detectify.Services;
using Detectify.Filters;
using Detectify.Utils;

namespace Detectify.Controllers;

/// <summary>
/// API controller for managing S3 storage interaction.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class S3Controller : ControllerBase, IDisposable
{
    /// <summary>
    /// <see cref="StorageService"/> implementation to be used.
    /// </summary>
    public readonly IStorageService _storageService;
    public readonly ILogger _logger;

    public S3Controller(IStorageService storageService, ILogger<S3Controller> logger) =>
        (_storageService, _logger) = (storageService, logger);

    /// <summary>
    /// Uploads an object to the S3 bucket.
    /// </summary>
    /// <param name="filename">Represents the name of the file to be uploaded.</param>
    /// <param name="formFile">Represents the file to be uploaded.</param>
    /// <returns>An IActionResult indicating the status of the upload along with the name of the uploaded object.</returns>
    /// <response code="200">Returns the name of the uploaded object. Provided name should be used to identify rendered file on the other S3 bucket after lambda function processing.</response>
    /// <response code="400">Returns Bad Request response in case of client-side error.</response>
    /// <remarks>
    /// Sample request:
    /// curl -X POST
    ///   /api/s3/upload/sample.jpg
    ///   -H 'Content-Type: multipart/form-data'
    ///   -F 'formFile=@/path/to/local/sample.jpg'
    /// </remarks>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    [HttpPost("upload/{filename}")]
    [ServiceFilter(typeof(LoggingFilter))]
    public async Task<IActionResult> UploadObject([Required] string filename, [Required] IFormFile formFile)
    {
        try
        {
            await using var objectStream = new MemoryStream();
            await formFile.CopyToAsync(objectStream);
            var bucketObject = new S3Object()
            {
                Name = StorageServiceUtils.GenerateUUIDFilename(filename),
                InputStream = objectStream
            };
            await _storageService.UploadObjectAsync(bucketObject);
            _logger.LogInformation($"INT: Request results with \"{bucketObject.Name}\"");
            return Ok(bucketObject.Name);
        }
        catch (Exception ex)
        {
            return BadRequest($"Upload failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves an object from the S3 bucket.
    /// </summary>
    /// <param name="filename">Represents name of the file to be downloaded.</param>
    /// <returns>An IActionResult indicating the status of the upload along with the file containing the downloaded object.</returns>
    /// <response code="200">Returns <see cref="File"/> as a stream representing requested object. Name is always "pupserach-result". Response needs to be handled as Blob object (JS).</response>
    /// <response code="400">Returns Bad Request response in case of client-side error.</response>
    /// <response code="404">Returns Not Found response in case if requested file is not found on specified S3 bucket.</response>
    /// <remarks>
    /// Sample request:
    /// curl -X GET /api/s3/download
    ///   -H 'Content-Type: text/plain'
    ///   -F 'sample.jpg'
    /// </remarks>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    [HttpGet("download/{filename}")]
    [ServiceFilter(typeof(LoggingFilter))]
    public async Task<IActionResult> DownloadObject([Required] string filename)
    {
        try
        {
            var responseStreamAsync = await _storageService.DownloadObjectAsync(filename);
            string contentType = MimeTypeUtils.GetContentType(filename);
            return File(responseStreamAsync, contentType, "Detectify-result");
        }
        catch (FileNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Disposes <see cref="StorageService"/> instance.
    /// </summary>
    public void Dispose()
    {
        _storageService?.Dispose();
    }
}
