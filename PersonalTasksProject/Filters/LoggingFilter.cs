using Microsoft.AspNetCore.Mvc.Filters;

namespace PersonalTasksProject.Filters;

public class LoggingFilter : IAsyncActionFilter
{
    private readonly ILogger<LoggingFilter> _logger;
    public LoggingFilter(ILogger<LoggingFilter> logger)
    {
        _logger = logger;
    }
    
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var httpContext = context.HttpContext;
        var method = httpContext.Request.Method;
        var endpointRoute = httpContext.GetEndpoint().Metadata.FirstOrDefault(item => item.ToString().Contains("Route:")).ToString().Split("Route: ")[1].ToUpper();

        _logger.LogInformation($"Executing Endpoint: [{method}] {endpointRoute}");
        await next();
    }
}