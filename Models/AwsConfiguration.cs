namespace Detectify.Models;

/// <summary>
/// Represents necessary configuration settings related to AWS services.
/// </summary>
public class AwsConfiguration
{
    private static IConfiguration _configuration = null!;
    private static string _nullDeserializationExceptionMessage =
        "Configuration property cannot be null. Verify that all values are present in appsettings.json configuration file.";

    /// <summary>
    /// Initializes a new instance of <see cref="AwsConfiguration"/> class.
    /// </summary>
    /// <param name="configuration"><see cref="IConfiguration"/> instance to read configuration values from.</param>
    public AwsConfiguration(IConfiguration configuration)
    {
        _configuration = configuration.GetSection("Aws");
        AccessKey = _configuration["AccessKey"] ?? throw new Exception(_nullDeserializationExceptionMessage);
        SecretKey = _configuration["SecretAccessKey"] ?? throw new Exception(_nullDeserializationExceptionMessage);
        S3Buckets = new S3Bucket();
        LambdaFunctions = new LambdaFunction();
        Region = _configuration["Region"] ?? throw new Exception(_nullDeserializationExceptionMessage);
    }

    /// <summary>
    /// Represents Aws Access Key.
    /// </summary>
    public string AccessKey { get; private set; }
    /// <summary>
    /// Represents Aws Secret Key.
    /// </summary>
    public string SecretKey { get; private set; }
    /// <summary>
    /// Represents a collection of avaliable S3 buckets.
    /// </summary>
    public S3Bucket S3Buckets { get; private set; }
    /// <summary>
    /// Represents a collection of avaliable Lambda functions.
    /// </summary>
    public LambdaFunction LambdaFunctions { get; private set; }
    /// <summary>
    /// Represents the Aws Region to be used.
    /// </summary>
    public string Region { get; private set; }

    /// <summary>
    /// Represents configuration settings for the available Lambda functions.
    /// </summary>
    public record LambdaFunction
    {
        public LambdaFunction() =>
            GetImageLabels = _configuration["Lambda:Label"] ?? throw new Exception(_nullDeserializationExceptionMessage);

        /// <summary>
        /// Represents the name of the Lambda function used to retrieve image labels.
        /// </summary>
        public string GetImageLabels { get; init; }
    }
    
    /// <summary>
    /// Represents configuration settings for the available S3 buckets.
    /// </summary>
    public record S3Bucket
    {
        public S3Bucket()
        {
            Put = _configuration["S3Bucket:Put"] ?? throw new Exception(_nullDeserializationExceptionMessage);
            Get = _configuration["S3Bucket:Get"] ?? throw new Exception(_nullDeserializationExceptionMessage);
        }

        public string Put { get; init; }
        public string Get { get; init; }
    }
}