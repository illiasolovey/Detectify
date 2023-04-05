using PupSearch.Filters;
using PupSearch.Models;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.ComponentModel.DataAnnotations;
using PupsearchShared.Models;
using Newtonsoft.Json;

namespace PupSearch.Controllers;

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
    /// <returns>An IActionResult indicating the status of the upload along with the <see cref="ContentResult"/> containing the pre-signed URL to the rendered media file.</returns>
    /// <response code="200">Returns pre-signed url to rendered media file on "Get" bucket as <see cref="ContentResult"/> object.</response>
    /// <response code="500">Returns "Internal Server Error" response in case of any AWS related exceptions along with function name and exception message.</response>
    /// <remarks>
    /// Sample request:
    /// curl -X GET /api/lambda/object-analysis
    ///   -H 'Content-Type: text/plain'
    ///   -F 'sample.jpg'
    /// </remarks>
    [HttpGet("object-analysis")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    [ServiceFilter(typeof(CacheFilter))]
    public async Task<IActionResult> InvokeObjectAnalysis([Required] string filename, [Required, Range(0, 100)] float confidencePercentage)
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
            Confidence = confidencePercentage
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
