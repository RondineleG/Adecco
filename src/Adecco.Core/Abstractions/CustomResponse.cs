namespace Adecco.Core.Abstractions;

public class CustomResponse
{
    public string Id { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Message { get; set; } = string.Empty;

    public CustomResponse()
    {
        Status = CustomResultStatus.Success;
    }

    public CustomResponse(string message)
    {
        AddError(message);
    }

    public CustomResponse(string id, string message)
    {
        Id = id;
        Date = DateTime.Now;
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
}
