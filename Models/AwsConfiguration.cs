namespace PupSearch.Models;

public class AwsConfiguration
{
    private static IConfiguration _configuration = null!;
    private static string _nullDeserializationExceptionMessage =
        "Configuration property cannot be null. Verify that all values are present in appsettings.json configuration file.";

    public AwsConfiguration(IConfiguration configuration)
    {
        _configuration = configuration.GetSection("Aws");
        AccessKey = _configuration["AccessKey"] ?? throw new Exception(_nullDeserializationExceptionMessage);
        SecretKey = _configuration["SecretAccessKey"] ?? throw new Exception(_nullDeserializationExceptionMessage);
        S3Buckets = new S3Bucket();
        LambdaFunctions = new LambdaFunction();
        Region = _configuration["Region"] ?? throw new Exception(_nullDeserializationExceptionMessage);
    }

    public string AccessKey { get; private set; }
    public string SecretKey { get; private set; }
    public S3Bucket S3Buckets { get; private set; }
    public LambdaFunction LambdaFunctions { get; private set; }
    public string Region { get; private set; }

    public class LambdaFunction
    {
        public LambdaFunction() =>
            GetImageLabels = _configuration["Lambda:Label"] ?? throw new Exception(_nullDeserializationExceptionMessage);

        public string GetImageLabels { get; private set; }
    }

    public class S3Bucket
    {
        public S3Bucket() =>
            Put = _configuration["S3Bucket:Put"] ?? throw new Exception(_nullDeserializationExceptionMessage);

        public string Put { get; private set; }
    }
}