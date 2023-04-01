using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using PupSearch.Models;
using PupSearch.Services;

namespace PupSearch.Controllers;

[ApiController]
[Route("api/[controller]")]
public class S3Controller : ControllerBase
{
    private readonly AwsConfiguration _awsConfiguration;
    private StorageService _storageService = null!;

    public S3Controller(AwsConfiguration awsConfiguration) =>
        _awsConfiguration = awsConfiguration;

    [HttpPost("upload/{filename}")]
    public async Task<IActionResult> UploadObject([Required] string filename, [Required] IFormFile formFile)
    {
        try
        {
            await using var objectStream = new MemoryStream();
            await formFile.CopyToAsync(objectStream);
            var bucketObject = new S3Object()
            {
                Name = filename,
                InputStream = objectStream
            };
            _storageService = new StorageService(_awsConfiguration);
            await _storageService.UploadObjectAsync(bucketObject);
            return Ok(bucketObject.Name);
        }
        catch (Exception ex)
        {
            return BadRequest($"Upload failed: {ex.Message}");
        }
    }
}