namespace Adecco.Core.Abstractions;

public class ApiErrorResponse
{
    public string Id { get; set; }
    public DateTime Date { get; set; }
    public string Message { get; set; }

    public ApiErrorResponse(string id)
    {
        Id = id;
        Date = DateTime.Now;
        Message = "Erro inesperado.";
    }
}