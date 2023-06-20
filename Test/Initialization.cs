namespace Detectify.Tests;

static class Initialization
{
    public static AwsConfiguration AwsConfigurationFromEnvironmentVariables()
    {
        var _awsConfiguration = new AwsConfiguration(
            accessKey: Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID")!,
            secretKey: Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY")!,
            region: Environment.GetEnvironmentVariable("AWS_REGION")!,
            lambdaFunctions: new Dictionary<string, string>
            {
                 { "ObjectAnalysis", Environment.GetEnvironmentVariable("AWS_LAMBDA_OBJECT_ANALYSIS")! },
                 { "Celebrity", Environment.GetEnvironmentVariable("AWS_LAMBDA_CELEBRITY_RECOGNITION")! }
            },
            s3Buckets: new Dictionary<string, string>
            {
                { "Put", Environment.GetEnvironmentVariable("AWS_S3_INPUT_BUCKET")! },
                { "Get", Environment.GetEnvironmentVariable("AWS_S3_OUTPUT_BUCKET")! }
            }
        );

        return _awsConfiguration;
    }
}