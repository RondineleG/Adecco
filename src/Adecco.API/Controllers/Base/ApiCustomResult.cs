namespace Adecco.API.Controllers.Base;

public sealed class ApiCustomResult
{
    public HttpStatusCode StatusCode { get; private set; }
    public bool Success { get; private set; }
    public object Data { get; private set; }
    public IEnumerable<string> Errors { get; private set; }

    public ApiCustomResult(HttpStatusCode statusCode, bool success)
    {
        StatusCode = statusCode;
        Success = success;
    }

    public ApiCustomResult(HttpStatusCode statusCode, bool success, object data)
        : this(statusCode, success) => Data = data;

    public ApiCustomResult(HttpStatusCode statusCode, bool success, IEnumerable<string> errors)
        : this(statusCode, success) => Errors = errors;

    public ApiCustomResult(
        HttpStatusCode statusCode,
        bool success,
        object data,
        IEnumerable<string> errors
    )
        : this(statusCode, success, data) => Errors = errors;
}
