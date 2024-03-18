namespace Adecco.Core.Abstractions;

public abstract class BaseResponse(bool success, string message)
{
    public bool Success { get; protected set; } = success;

    public string Message { get; protected set; } = message;
}
