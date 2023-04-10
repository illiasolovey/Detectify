using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace Detectify.Filters;

/// <summary>
/// Caches the output of an action method.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class CacheFilter : Attribute, IAsyncActionFilter
{
    private readonly IMemoryCache _cache;
    private readonly TimeSpan _cacheDurationSeconds;
    
    public CacheFilter(IMemoryCache cache, TimeSpan cacheDurationSeconds) =>
        (_cache, _cacheDurationSeconds) = (cache, cacheDurationSeconds);

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        string cacheKey = GetCacheKey(context);
        if (_cache.TryGetValue(cacheKey, out string? cachedResult))
        {
            context.Result = new ContentResult { Content = cachedResult, ContentType = "application/json" };
            return;
        }

        ActionExecutedContext executedContext = await next();

        if (executedContext.Exception == null && executedContext.Result is ContentResult contentResult)
            _cache.Set(cacheKey, contentResult.Content, _cacheDurationSeconds);
        else
            throw new InvalidOperationException("Unable to cache the result because the wrapped function does not return ContentResult.");
    }

    private string GetCacheKey(ActionExecutingContext context)
    {
        string actionName = context.ActionDescriptor.DisplayName
            ?? throw new ArgumentNullException(nameof(context.ActionDescriptor), "ActionDescriptor cannot be null."); ;
        string queryString = string.Join(separator: "&", context.HttpContext.Request.Query.Select(x => $"{x.Key}={x.Value}"));
        return $"{actionName}-{queryString}";
    }
}