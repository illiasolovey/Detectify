using Detectify.Filters;
using Detectify.Models;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.ComponentModel.DataAnnotations;
using DetectifyShared.Models;
using Newtonsoft.Json;

namespace Detectify.Controllers;

/// <summary>
/// Provides endpoints for invoking AWS Lambda functions.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class LambdaController : ControllerBase
{
    private readonly AwsConfiguration _awsConfiguration;
    public LambdaController(AwsConfiguration awsConfiguration) =>
        _awsConfiguration = awsConfiguration;

    /// <summary>
    /// Invokes "Object Analysis" Lambda function.
    /// </summary>
    /// <param name="filename">Name of bucket object to analyze.</param>
    /// <param name="confidencePercentage">Percentage of accuracy of the performed label detection.</param>
    /// <param name="boundingBoxHex">Hexadecimal color code (e.g. "#RRGGBB") representing the color of the bounding box.</param>
    /// <param name="labelHex">Hexadecimal color code (e.g. "#RRGGBB") representing the color of the label displayed inside bounding box.</param>
    /// <returns>An IActionResult indicating the status of the upload along with the <see cref="ContentResult"/> containing the pre-signed URL to the rendered media file.</returns>
    /// <response code="200">Returns pre-signed url to rendered media file on "Get" bucket as <see cref="ContentResult"/> object.</response>
    /// <response code="500">Returns "Internal Server Error" response in case of any AWS related exceptions along with function name and exception message.</response>
    /// <remarks>
    /// Sample request:
    /// curl -X GET /api/lambda/object-analysis
    ///   -H 'Content-Type: text/plain'
    ///   -F 'sample.jpg'
    /// </remarks>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    [HttpGet("object-analysis")]
    [ServiceFilter(typeof(LoggingFilter))]
    [ServiceFilter(typeof(CacheFilter))]
    public async Task<IActionResult> InvokeObjectAnalysis([Required] string filename, [Required, Range(0, 100)] float confidencePercentage, string boundingBoxHex, string labelHex)
    {
        string functionName = _awsConfiguration.LambdaFunctions.GetImageLabels;
        using AmazonLambdaClient lambdaClient = new(
            awsAccessKeyId: _awsConfiguration.AccessKey,
            awsSecretAccessKey: _awsConfiguration.SecretKey,
            region: Amazon.RegionEndpoint.GetBySystemName(_awsConfiguration.Region)
        );
        LambdaPayload payload = new()
        {
            ObjectKey = filename,
            Confidence = confidencePercentage,
            BoundingBoxColorHEX = boundingBoxHex,
            LabelColorHEX = labelHex
        };
        string payloadJson = JsonConvert.SerializeObject(payload);
        InvokeRequest invokeRequest = new()
        {
            InvocationType = InvocationType.RequestResponse,
            FunctionName = functionName,
            Payload = payloadJson
        };
        try
        {
            InvokeResponse functionResponse = await lambdaClient.InvokeAsync(invokeRequest);
            var responseContent = Encoding.ASCII.GetString(functionResponse.Payload.ToArray());
            return new ContentResult
            {
                Content = responseContent,
                ContentType = "text/plain",
                StatusCode = 200
            };
        }
        catch (AmazonLambdaException ex)
        {
            return StatusCode(500, $"Internal error occured on attempt to invoke \"{functionName}\": {ex.Message}");
        }
    }
}
