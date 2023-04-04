using PupSearch.Models;

namespace PupSearch.Services;

/// <summary>
/// Handles storing and retrieving objects from AWS S3.
/// </summary>
public interface IStorageService : IDisposable
{
    /// <summary>
    /// Uploads object to S3 using TransferUtility.
    /// </summary>
    /// <param name="s3Object"><see cref="S3Object"/> instance to be uploaded.</param>
    public Task UploadObjectAsync(S3Object s3Object);
    /// <summary>
    /// Retrieves object from S3 using GetObjectRequest.
    /// </summary>
    /// <param name="filename">Object key of document to be retrieved.</param>
    /// <returns>Byte array representing retrieved object.</returns>
    public Task<byte[]> DownloadObjectAsync(string filename);
}