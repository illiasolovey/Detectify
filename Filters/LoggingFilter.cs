using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace PupSearch.Filters;

/// <summary>
/// Logs necessary information on and after every action method execution.
/// </summary>
public class LoggingFilter : Attribute, IActionFilter
{
    private readonly ILogger<LoggingFilter> _logger;
    public LoggingFilter(ILogger<LoggingFilter> logger) =>
        _logger = logger;

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var methodName = context.ActionDescriptor.DisplayName;
        var arguments = JsonConvert.SerializeObject(context.ActionArguments);
        var remoteIpAddress = context.HttpContext.Connection.RemoteIpAddress;
        var xForwardedFor = context.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault("No redirections detected.");
        var dateTime = DateTime.UtcNow.ToString("o");
        _logger.LogInformation($"[{dateTime}] {methodName} - Upload request from {remoteIpAddress}, X-Forwarded-For: {xForwardedFor}");
        _logger.LogInformation($"{methodName} - Argumets taken: {arguments}");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        var methodName = context.ActionDescriptor.DisplayName;
        var remoteIpAddress = context.HttpContext.Connection.RemoteIpAddress;
        var dateTime = DateTime.UtcNow.ToString("o");
        if (context.Exception != null)
            _logger.LogError($"[{dateTime}] {methodName} - Request from {remoteIpAddress} resulted an exception: {context.Exception.Message}");
        _logger.LogInformation($"[{dateTime}] {methodName} - Request from {remoteIpAddress} handled successfully.");
    }
}