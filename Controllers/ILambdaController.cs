using System.ComponentModel.DataAnnotations;
using Detectify.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Detectify.Models;

public interface ILambdaController
{
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
    Task<IActionResult> InvokeObjectAnalysis([Required] string filename, [Required, Range(0, 100)] float confidencePercentage, string boundingBoxHex, string labelHex);
}