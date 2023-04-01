using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using PupSearch.Models;

namespace PupSearch.Services
{
    public class StorageService
    {
        private readonly string _bucketName;
        private readonly AmazonS3Client _awsS3Client;

        public StorageService(AwsConfiguration awsConfiguration)
        {
            _bucketName = awsConfiguration.S3Buckets.Put;
            _awsS3Client = new AmazonS3Client(
                awsConfiguration.AccessKey,
                awsConfiguration.SecretKey,
                RegionEndpoint.GetBySystemName(awsConfiguration.Region)
            );
        }

        public async Task UploadObjectAsync(S3Object s3Object)
        {
            var transferUtility = new TransferUtility(_awsS3Client);
            var uploadRequest = new TransferUtilityUploadRequest()
            {
                InputStream = s3Object.InputStream,
                Key = s3Object.Name,
                BucketName = _bucketName,
                CannedACL = S3CannedACL.NoACL
            };
            await transferUtility.UploadAsync(uploadRequest);
        }
    }
}