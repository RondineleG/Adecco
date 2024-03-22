namespace Adecco.Core.Abstractions;

public sealed class CustomValidationResult
{
    private readonly List<string> _errors = new List<string>();

    public IEnumerable<string> Errors => new List<string>(_errors);          
    public bool IsValid => !_errors.Any();

    public CustomValidationResult AddError(string errorMessage, string fieldName = "")
    {
        if (!string.IsNullOrWhiteSpace(errorMessage))
        {
            _errors.Add(string.IsNullOrWhiteSpace(fieldName) ? errorMessage : $"{fieldName}: {errorMessage}");
        }
        return this;
    }

    public CustomValidationResult AddErrorIf(bool condition, string errorMessage, string fieldName = "")
    {
        if (condition)
        {
            AddError(errorMessage, fieldName);
        }
        return this;
    }

    public CustomValidationResult Merge(CustomValidationResult result)
    {
        foreach (var erro in result.Errors)
        {
            AddError(erro);
        }
        return this;
    }
}