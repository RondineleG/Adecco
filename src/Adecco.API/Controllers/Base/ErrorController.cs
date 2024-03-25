namespace Adecco.API.Controllers.Base;

[ApiExplorerSettings(IgnoreApi = true)]
[ApiController]
public sealed class ErrorController : ControllerBase
{
    [Route("error")]
    public ApiErrorResponse Error()
    {
        Response.StatusCode = 500;
        var id = Activity.Current?.Id ?? HttpContext?.TraceIdentifier;
        return new ApiErrorResponse(id);
    }
}
