namespace Adecco.Core.Abstractions;

public class ContatoResponse : BaseResponse
{
    public ContatoResponse(Contato contato) : this(true, string.Empty, contato)
    {
    }

    public ContatoResponse(string message) : this(false, message, null)
    {
    }

    private ContatoResponse(bool success, string message, Contato contato) : base(success, message)
    {
        Contato = contato;
    }

    public Contato Contato { get; private set; }
}