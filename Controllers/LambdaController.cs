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
public class LambdaController : ControllerBase, ILambdaController
{
    private readonly IAwsConfiguration _awsConfiguration;
    public LambdaController(IAwsConfiguration awsConfiguration) =>
        _awsConfiguration = awsConfiguration;

    /// <inheritdoc/>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    [HttpGet("object-analysis")]
    [ServiceFilter(typeof(LoggingFilter))]
    [ServiceFilter(typeof(CacheFilter))]
    public async Task<IActionResult> InvokeObjectAnalysis(string filename, [Required, Range(0, 100)] float confidencePercentage, string boundingBoxHex, string labelHex)
    {
        if (string.IsNullOrWhiteSpace(filename))
            return BadRequest("Filename is a required property and cannot be null or empty.");
            
        string functionName = _awsConfiguration.LambdaFunctions["ObjectAnalysis"];
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
