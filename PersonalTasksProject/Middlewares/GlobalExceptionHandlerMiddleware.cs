using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace PersonalTasksProject.Middlewares;

public class GlobalExceptionHandlerMiddleware : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _logger = logger;
    }
    
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken
        )
    {
        _logger.LogError(exception, "Exception occuredL {exception.Message}", exception.Message);
        
        var (statusCode, title) = MapException(exception);
        
        var problemsDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
        };
            
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemsDetails, cancellationToken);

        return true;
    }
    
    private static (int StatusCode, string Title) MapException(Exception exception)
    {
        return exception switch
        {
            SecurityTokenExpiredException => ( StatusCodes.Status401Unauthorized, exception.Message ),
            SecurityTokenException => ( StatusCodes.Status401Unauthorized, exception.Message ),
            _ => ( StatusCodes.Status500InternalServerError,
                "Something went wrong but we are working on it!" )
        };
    }
}