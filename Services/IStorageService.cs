namespace PupSearch.Services;

public interface IStorageService : IDisposable
{
    public Task UploadObjectAsync(Models.S3Object s3Object);
    public Task<byte[]> DownloadObjectAsync(string filename);
}