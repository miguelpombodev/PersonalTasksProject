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
        _logger.LogInformation("Executing action {action}", context.ActionDescriptor.DisplayName);
        await next();
    }
}