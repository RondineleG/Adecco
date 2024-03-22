namespace Adecco.Core.Abstractions;

public sealed class CustomResponse
{
    public CustomResponse()
    {
        Success = true;
    }

    public bool Success { get; set; }
    public List<string> GeneralErrors { get; set; } = [];

    public Dictionary<string, List<string>> EntityErrors { get; set; } = [];

    public void AddError(string message)
    {
        Success = false;
        GeneralErrors.Add(message);
    }

    public void AddEntityError(string entity, string message)
    {
        Success = false;
        if (!EntityErrors.TryGetValue(entity, out var value))
        {
            value = [];
            EntityErrors[entity] = value;
        }

        value.Add(message);
    }

    public override string ToString()
    {
        var messages = new List<string>();

        if (GeneralErrors.Any())
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
}
