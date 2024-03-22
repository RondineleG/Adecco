namespace Adecco.Core.Exceptions;

public class BadRequestException : Exception
{
    private readonly List<string> _list = new List<string>();
    private readonly CustomResponse _validacaoResponse = new CustomResponse();

    public BadRequestException(string message)
        : base(message) { }

    public BadRequestException(List<string> list)
    {
        _list = list ?? throw new ArgumentNullException(nameof(list), "A lista não pode ser nula.");
    }

    public BadRequestException(CustomResponse validacaoResponse)
    {
        _validacaoResponse =
            validacaoResponse
            ?? throw new ArgumentNullException(
                nameof(validacaoResponse),
                "A resposta de validação não pode ser nula."
            );
    }
}
