using Adecco.Core.Exceptions;

using Microsoft.AspNetCore.Mvc.Filters;

using static Adecco.Core.Abstractions.CustomResult;

namespace Adecco.API.Filters;

public class ExceptionFilter(CustomResultConstructor customResultConstructor) : IExceptionFilter
{
    private readonly CustomResultConstructor _customResultConstructor = customResultConstructor;

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
            context.Result = new NotFoundObjectResult(_customResultConstructor("XA100", DateTime.Now, context.Exception.Message));
        }
        if (context.Exception is ErrorOnValidationException)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Result = new BadRequestObjectResult(_customResultConstructor("XA200", DateTime.Now, context.Exception.Message));
        }
        if (context.Exception is ConflictException)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
            context.Result = new ConflictObjectResult(_customResultConstructor("XA300", DateTime.Now, context.Exception.Message));
        }
    }

    private void ThrowUnkowError(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Result = new ObjectResult(_customResultConstructor("XA400", DateTime.Now, "ERR_INTERNAL_SERVER_ERROR"));
    }
}
