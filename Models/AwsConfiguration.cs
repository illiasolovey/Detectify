namespace Detectify.Models;

/// <summary>
/// Represents necessary configuration settings related to AWS services.
/// </summary>
public class AwsConfiguration : IAwsConfiguration
{
    private static IConfiguration _configuration = null!;
    private static string _nullDeserializationExceptionMessage =
        "Configuration property cannot be null. Verify that all values are present in appsettings.json configuration file.";

    /// <inheritdoc/>
    public string AccessKey { get; private set; }
    /// <inheritdoc/>
    public string SecretKey { get; private set; }
    /// <inheritdoc/>
    public string Region { get; private set; }
    /// <inheritdoc/>
    public Dictionary<string, string> LambdaFunctions { get; private set; }
    /// <inheritdoc/>
    public Dictionary<string, string> S3Buckets { get; private set; }

    /// <summary>
    /// Initializes a new instance of <see cref="AwsConfiguration"/> class.
    /// </summary>
    /// <param name="configuration"><see cref="IConfiguration"/> instance to read configuration values from.</param>
    public AwsConfiguration(IConfiguration configuration)
    {
        _configuration = configuration.GetSection("Aws");
        AccessKey = _configuration["AccessKey"] ?? throw new ArgumentNullException(_nullDeserializationExceptionMessage);
        SecretKey = _configuration["SecretAccessKey"] ?? throw new ArgumentNullException(_nullDeserializationExceptionMessage);
        Region = _configuration["Region"] ?? throw new ArgumentNullException(_nullDeserializationExceptionMessage);
        LambdaFunctions = ReadDictionaryFromConfiguration("Lambda");
        S3Buckets = ReadDictionaryFromConfiguration("S3Bucket");
    }

    public AwsConfiguration(string accessKey, string secretKey, string region, Dictionary<string, string> lambdaFunctions, Dictionary<string, string> s3Buckets)
    {
        AccessKey = accessKey;
        SecretKey = secretKey;
        Region = region;
        LambdaFunctions = lambdaFunctions;
        S3Buckets = s3Buckets;
    }

    private Dictionary<string, string> ReadDictionaryFromConfiguration(string sectionName)
    {
        var dictionary = new Dictionary<string, string>();
        var section = _configuration.GetSection(sectionName);
        foreach (var childSection in section.GetChildren())
            dictionary[childSection.Key] = childSection.Value ?? throw new ArgumentNullException(_nullDeserializationExceptionMessage);
        return dictionary;
    }
}