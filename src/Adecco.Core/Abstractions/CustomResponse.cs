namespace Adecco.Core.Abstractions;

public class CustomResponse : BaseResponse
{
    public List<string> GeneralErrors { get; private set; } = new List<string>();
    public Dictionary<string, List<string>> EntityErrors { get; private set; } =
        new Dictionary<string, List<string>>();

    public CustomResponse(bool success, string message)
        : base(success, message) { }

    public void AddGeneralError(string message)
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
        var messages = new List<string> { Message };

        messages.AddRange(GeneralErrors);

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
