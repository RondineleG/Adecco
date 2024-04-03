namespace Adecco.Core.Abstractions;

public enum CustomResultStatus
{
    Success,
    HasValidation,
    HasError,
    EntityNotFound,
    EntityAlreadyExists,
    NoContent
}

public interface ICustomResult
{
    CustomResultStatus Status { get; }
}

public interface ICustomResult<out T> : ICustomResult
{
    T? Data { get; }
}

public interface ICustomResultValidations : ICustomResult
{
    IEnumerable<Validation> Validations { get; }
}

public interface ICustomResultError : ICustomResult
{
    Error? Error { get; }
}

public interface IRequestEntityWarning : ICustomResult
{
    EntityWarning? EntityWarning { get; }
}

public class CustomCustomResult : ICustomResultValidations, ICustomResultError, IRequestEntityWarning
{
    public static CustomCustomResult Success()
        => new CustomCustomResult { Status = CustomResultStatus.Success };
    public static CustomCustomResult WithNoContent()
        => new CustomCustomResult { Status = CustomResultStatus.NoContent };
    public static CustomCustomResult EntityNotFound(string entity, object id, string description)
        => new()
        {
            Status = CustomResultStatus.EntityNotFound,
            EntityWarning = new EntityWarning(entity, id, description)
        };
    public static CustomCustomResult EntityAlreadyExists(string entity, object id, string description)
        => new()
        {
            Status = CustomResultStatus.EntityAlreadyExists,
            EntityWarning = new EntityWarning(entity, id, description)
        };
    public static CustomCustomResult WithError(string message)
        => new()
        {
            Status = CustomResultStatus.HasError,
            Error = new Error(message)
        };
    public static CustomCustomResult WithError(Exception exception)
        => WithError(exception.Message);
    public static CustomCustomResult WithValidations(params Validation[] validations)
        => new()
        {
            Status = CustomResultStatus.HasValidation,
            Validations = validations
        };
    public static CustomCustomResult WithValidations(IEnumerable<Validation> validations)
        => WithValidations(validations.ToArray());
    public static CustomCustomResult WithValidations(string propertyName, string description)
        => WithValidations(new Validation(propertyName, description));

    public CustomResultStatus Status { get; protected init; }

    public IEnumerable<Validation> Validations { get; protected init; } = Enumerable.Empty<Validation>();

    public Error? Error { get; protected init; }

    public EntityWarning? EntityWarning { get; protected init; }
}

public class CustomResult<T> : CustomCustomResult, ICustomResult<T>
{
    public static CustomResult<T> Success(T data)
        => new()
        {
            Data = data,
            Status = CustomResultStatus.Success
        };
    public new static CustomResult<T> WithNoContent()
        => new()
        {
            Status = CustomResultStatus.NoContent
        };
    public new static CustomResult<T> EntityNotFound(string entity, object id, string description)
        => new()
        {
            Status = CustomResultStatus.EntityNotFound,
            EntityWarning = new EntityWarning(entity, id, description)
        };
    public new static CustomResult<T> EntityAlreadyExists(string entity, object id, string description)
        => new()
        {
            Status = CustomResultStatus.EntityAlreadyExists,
            EntityWarning = new EntityWarning(entity, id, description)
        };
    public new static CustomResult<T> WithError(string message)
        => new()
        {
            Status = CustomResultStatus.HasError,
            Error = new Error(message)
        };
    public new static CustomResult<T> WithError(Exception exception)
        => WithError(exception.Message);
    public new static CustomResult<T> WithValidations(params Validation[] validations)
        => new()
        {
            Status = CustomResultStatus.HasValidation,
            Validations = validations
        };
    public new static CustomResult<T> WithValidations(string propertyName, string description)
        => WithValidations(new Validation(propertyName, description));

    public T? Data { get; private init; }

    public static implicit operator CustomResult<T>(T data) => Success(data);
    public static implicit operator CustomResult<T>(Exception ex) => WithError(ex);
    public static implicit operator CustomResult<T>(Validation[] validations) => WithValidations(validations);
    public static implicit operator CustomResult<T>(Validation validation) => WithValidations(validation);
}

public record Validation(string PropertyName, string Description);

public record Error(string Description);

public record EntityWarning(string Name, object Id, string Message);

public class CustomResultException(CustomCustomResult customResult) : Exception
{
    public CustomCustomResult CustomResult => customResult;

    public CustomResultException(params Validation[] validations)
        : this(CustomCustomResult.WithValidations(validations)) { }

    public CustomResultException(Exception exception)
        : this(CustomCustomResult.WithError(exception)) { }
}