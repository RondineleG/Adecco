using Adecco.Core.Exceptions;

using Microsoft.AspNetCore.Mvc.Filters;

namespace Adecco.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var result = context.Exception is CustomException;
        if (result)
        {
            HandleProjectException(context);
        }
        else
        {
            ThrowUnkowError(context);
        }
    }

    private void HandleProjectException(ExceptionContext context)
    {
        if (context.Exception is NotFoundException)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            context.Result = new NotFoundObjectResult(new CustomResult("XA100", context.Exception.Message));
        }
        if (context.Exception is ErrorOnValidationException)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Result = new BadRequestObjectResult(new CustomResult("XA200", context.Exception.Message));
        }
        if (context.Exception is ConflictException)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
            context.Result = new ConflictObjectResult(new CustomResult("XA100", context.Exception.Message));
        }
    }

    private void ThrowUnkowError(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Result = new ObjectResult(new CustomResult("XA400", "ERR_INTERNAL_SERVER_ERROR"));
    }
}