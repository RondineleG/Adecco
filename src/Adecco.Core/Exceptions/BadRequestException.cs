

namespace Adecco.Core.Exceptions;


public class BadRequestException : Exception
{
    private readonly List<string> _list;
    private readonly CustomResponse _validacaoResponse;

    public BadRequestException(string message) : base(message) { }

    public BadRequestException(List<string> list) => _list = list;
    public BadRequestException(CustomResponse validacaoResponse) => _validacaoResponse = validacaoResponse;
}
