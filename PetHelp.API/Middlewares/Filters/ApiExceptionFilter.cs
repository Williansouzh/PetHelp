using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using PetHelp.Infra.Data.Exceptions;

namespace PetHelp.API.Middlewares.Filters;

public class ApiExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ApiExceptionFilter> _logger;

    public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;
        ObjectResult result;

        switch (exception)
        {
            case RoleNotFoundException ex:
                result = new ObjectResult(new
                {
                    Message = ex.Message
                })
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
                break;

            // você pode adicionar mais casos aqui
            // case ValidationException ex:
            // case UnauthorizedAccessException ex:

            default:
                _logger.LogError(exception, "Unhandled exception.");
                result = new ObjectResult(new
                {
                    Message = "An error occurred while processing your request.",
                    Details = exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
                break;
        }

        context.Result = result;
        context.ExceptionHandled = true;
    }
}
