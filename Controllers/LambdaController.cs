using PupSearch.Filters;
using PupSearch.Models;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace PupSearch.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LambdaController : ControllerBase
{
    private readonly AwsConfiguration _awsConfiguration;
    public LambdaController(AwsConfiguration awsConfiguration) =>
        _awsConfiguration = awsConfiguration;

    [HttpGet("object-analysis")]
    [ServiceFilter(typeof(CacheFilter))]
    public async Task<IActionResult> ObjectAnalysis([Required] string filename)
    {
        string functionName = _awsConfiguration.LambdaFunctions.GetImageLabels;
        using AmazonLambdaClient lambdaClient = new(
            awsAccessKeyId: _awsConfiguration.AccessKey,
            awsSecretAccessKey: _awsConfiguration.SecretKey,
            region: Amazon.RegionEndpoint.GetBySystemName(_awsConfiguration.Region)
        );
        InvokeRequest invokeRequest = new()
        {
            InvocationType = InvocationType.RequestResponse,
            FunctionName = functionName,
            Payload = $"\"{filename}\""
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
