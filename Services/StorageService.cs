using System.Net;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using PupSearch.Models;

namespace PupSearch.Services
{
    public class StorageService : IStorageService, IDisposable
    {
        private readonly string _putBucket;
        private readonly string _getBucket;
        private readonly AmazonS3Client _awsS3Client;

        public StorageService(AwsConfiguration awsConfiguration)
        {
            _putBucket = awsConfiguration.S3Buckets.Put;
            _getBucket = awsConfiguration.S3Buckets.Get;
            _awsS3Client = new AmazonS3Client(
                awsConfiguration.AccessKey,
                awsConfiguration.SecretKey,
                RegionEndpoint.GetBySystemName(awsConfiguration.Region)
            );
        }

        public async Task UploadObjectAsync(Models.S3Object s3Object)
        {
            try
            {
                var transferUtility = new TransferUtility(_awsS3Client);
                var uploadRequest = new TransferUtilityUploadRequest()
                {
                    InputStream = s3Object.InputStream,
                    Key = s3Object.Name,
                    BucketName = _putBucket,
                    CannedACL = S3CannedACL.NoACL
                };
                await transferUtility.UploadAsync(uploadRequest);
            }
            catch (Exception ex)
            {
                throw new Exception($"Upload failed: {ex.Message}");
            }
        }

        public async Task<byte[]> DownloadObjectAsync(string filename)
        {
            try
            {
                GetObjectRequest getObjectRequest = new GetObjectRequest
                {
                    BucketName = _getBucket,
                    Key = filename
                };
                using var response = await _awsS3Client.GetObjectAsync(getObjectRequest);
                if (response.HttpStatusCode == HttpStatusCode.OK)
                {
                    MemoryStream ms = new MemoryStream();
                    await response.ResponseStream.CopyToAsync(ms);
                    if (ms.Length == 0)
                        throw new Exception($"Requested object '{filename}' is empty.");
                    ms.Position = 0;
                    return ms.ToArray();
                }
                else if (response.HttpStatusCode == HttpStatusCode.NotFound)
                {
                    throw new FileNotFoundException($"Requested object '{filename}' not found.");
                }
                else
                {
                    throw new Exception($"Download failed: {response.HttpStatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Download failed: {ex.Message}");
            }
        }

        public void Dispose()
        {
            _awsS3Client?.Dispose();
        }
    }
}
