using System.Net;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Detectify.Models;

namespace Detectify.Services
{
    /// <summary>
    /// Provides an implementation of <see cref="IStorageService"/> interface to interact with a AWS storage service.
    /// </summary>
    public class StorageService : IStorageService, IDisposable
    {
        private readonly string _putBucket;
        private readonly string _getBucket;
        private readonly AmazonS3Client _awsS3Client;

        /// <summary>
        /// Initializes a new instance of <see cref="StorageService"/> class.
        /// </summary>
        /// <param name="awsConfiguration">DI registered service instance of <see cref="AwsConfiguration"/></param>

        public StorageService(AwsConfiguration awsConfiguration)
        {
            _putBucket = awsConfiguration.S3Buckets["Put"];
            _getBucket = awsConfiguration.S3Buckets["Get"];
            _awsS3Client = new AmazonS3Client(
                awsConfiguration.AccessKey,
                awsConfiguration.SecretKey,
                RegionEndpoint.GetBySystemName(awsConfiguration.Region)
            );
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
        public void Dispose()
        {
            _awsS3Client?.Dispose();
        }
    }
}
