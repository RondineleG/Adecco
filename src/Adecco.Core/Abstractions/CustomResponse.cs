namespace Adecco.Core.Abstractions;

public sealed class CustomResponse
{
    private CustomResponse _validacaoResponse;

    public CustomResponse()
    {
        Success = true;
    }

    public CustomResponse(CustomResponse validacaoResponse)
    {
        _validacaoResponse = validacaoResponse;
    }

    public bool Success { get; set; }
    public List<string> GeneralErrors { get; set; } = new List<string>();
    public Dictionary<string, List<string>> EntityErrors { get; set; } =
        new Dictionary<string, List<string>>();

    public void AddError(string message)
    {
        Success = false;
        GeneralErrors.Add(message);
    }

    public void AddEntityError(string entity, string message)
    {
        Success = false;
        if (!EntityErrors.ContainsKey(entity))
        {
            EntityErrors[entity] = new List<string>();
        }

        EntityErrors[entity].Add(message);
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
