namespace Adecco.Core.Abstractions;

public enum CustomResultStatus
{
    Success,
    HasValidation,
    HasError,
    EntityNotFound,
    EntityHasError,
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

public class CustomResult : ICustomResultValidations, ICustomResultError, IRequestEntityWarning
{
    public string Id { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Now;
    public string Message { get; set; } = string.Empty;

    public CustomResult()
    {
        Status = CustomResultStatus.Success;
    }

    public CustomResult(string message)
    {
        AddError(message);
    }

    public CustomResult(string id, string message)
    {
        Id = id;
        Message = message;
    }

    public CustomResultStatus Status { get; set; }
    public List<string> GeneralErrors { get; set; } = [];

    public Dictionary<string, List<string>> EntityErrors { get; set; } = [];

    public void AddError(string message)
    {
        Status = CustomResultStatus.HasError;
        GeneralErrors.Add(message);
    }

    public void AddEntityError(string entity, string message)
    {
        Status = CustomResultStatus.EntityHasError;
        if (!EntityErrors.TryGetValue(entity, out var value))
        {
            value = [];
            EntityErrors[entity] = value;
        }
        value.Add(message);
    }

    public delegate CustomResult CustomResultConstructor(string id, DateTime date, string message);
    public override string ToString()
    {
        var messages = new List<string>();

        if (GeneralErrors.Count != 0)
        {
            messages.AddRange(GeneralErrors);
        }

        foreach (var entityError in EntityErrors)
        {
            foreach (var error in entityError.Value)
            {
                messages.Add($"{entityError.Key}: {error}");
            }
        }
        return string.Join("; ", messages);
    }
    public static CustomResult Success()
        => new CustomResult { Status = CustomResultStatus.Success };
    public static CustomResult WithNoContent()
        => new CustomResult { Status = CustomResultStatus.NoContent };
    public static CustomResult EntityNotFound(string entity, object id, string description)
        => new()
        {                               
            Status = CustomResultStatus.EntityNotFound,
            EntityWarning = new EntityWarning(entity, id, description)
        };

    public static CustomResult EntityHasError(string entity, object id, string description)
     => new()
     {
         Status = CustomResultStatus.EntityHasError,
         EntityWarning = new EntityWarning(entity, id, description)
     };
    public static CustomResult EntityAlreadyExists(string entity, object id, string description)
        => new()
        {
            Status = CustomResultStatus.EntityAlreadyExists,
            EntityWarning = new EntityWarning(entity, id, description)
        };
    public static CustomResult WithError(string message)
        => new()
        {
            Status = CustomResultStatus.HasError,
            Error = new Error(message)
        };
    public static CustomResult WithError(Exception exception)
        => WithError(exception.Message);
    public static CustomResult WithValidations(params Validation[] validations)
        => new()
        {
            Status = CustomResultStatus.HasValidation,
            Validations = validations
        };
    public static CustomResult WithValidations(IEnumerable<Validation> validations)
        => WithValidations(validations.ToArray());
    public static CustomResult WithValidations(string propertyName, string description)
        => WithValidations(new Validation(propertyName, description));

    public IEnumerable<Validation> Validations { get; protected init; } = Enumerable.Empty<Validation>();

    public Error? Error { get; protected init; }

    public EntityWarning? EntityWarning { get; protected init; }
}

public class CustomResult<T> : CustomResult, ICustomResult<T>
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

    public new  static CustomResult<T> EntityHasError(string entity, object id, string description)
 => new()
 {
     Status = CustomResultStatus.EntityHasError,
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

public class CustomResultException(CustomResult customResult) : Exception
{
    public CustomResult CustomResult => customResult;

    public CustomResultException(params Validation[] validations)
        : this(CustomResult.WithValidations(validations)) { }

    public CustomResultException(Exception exception)
        : this(CustomResult.WithError(exception)) { }
}