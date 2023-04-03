using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using PupSearch.Models;
using PupSearch.Services;

namespace PupSearch.Controllers;

[ApiController]
[Route("api/[controller]")]
public class S3Controller : ControllerBase, IDisposable
{
    public readonly IStorageService _storageService;

    public S3Controller(IStorageService storageService) =>
        _storageService = storageService;

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
            await _storageService.UploadObjectAsync(bucketObject);
            return Ok(bucketObject.Name);
        }
        catch (Exception ex)
        {
            return BadRequest($"Upload failed: {ex.Message}");
        }
    }

    [HttpGet("download/{filename}")]
    public async Task<IActionResult> DownloadObject([Required] string filename)
    {
        try
        {
            var responseStreamAsync = await _storageService.DownloadObjectAsync(filename);
            string contentType = GetContentType(filename);
            return File(responseStreamAsync, contentType, "pupsearch-result");
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

    private string GetContentType(string filename)
    {
        string extension = Path.GetExtension(filename);
        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            _ => "application/octet-stream"
        };
    }

    public void Dispose()
    {
        _storageService?.Dispose();
    }
}
