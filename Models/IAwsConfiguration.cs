namespace Detectify.Models;

public interface IAwsConfiguration
{
    /// <summary>
    /// Represents Aws Access Key.
    /// </summary>
    string AccessKey { get; }
    /// <summary>
    /// Represents Aws Secret Key.
    /// </summary>
    string SecretKey { get; }
    /// <summary>
    /// Represents the Aws Region to be used.
    /// </summary>
    string Region { get; }
    /// <summary>
    /// Represents a colltion of avaliable   sdfsdf Lambda functions.
    /// </summary>
    Dictionary<string, string> LambdaFunctions { get; }
    /// <summary>
    /// Represents a collection of avaliable S3 buckets.
    /// </summary>
    Dictionary<string, string> S3Buckets { get; }
}