using Microsoft.AspNetCore.Mvc.Filters;

namespace PetHelp.API.Middlewares.Filters;


public class ApiLoggingFilter : IActionFilter
{
    private readonly ILogger<ApiLoggingFilter> _logger;

    public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation("Action executed: {ActionName}", context.ActionDescriptor.DisplayName);
        _logger.LogInformation("###################################################");
        _logger.LogInformation($"{DateTime.Now.ToLongTimeString}");
        _logger.LogInformation($"ModelState: {context.ModelState.IsValid}");
        _logger.LogInformation("###################################################");
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation("### exectuting -> OnActionExecuted");
        _logger.LogInformation("###################################################");
        _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
        _logger.LogInformation($"Status Code : {context.HttpContext.Response.StatusCode}");
        _logger.LogInformation("###################################################");

    }
}