namespace Adecco.Core.Abstractions;

public sealed class ContatoResponse : BaseResponse
{

    private ContatoResponse(bool success, string message, Contato contato)
        : base(success, message)
    {
        Contato = contato;
    }
    public ContatoResponse(Contato contato)
        : this(true, string.Empty, contato) { }

    public ContatoResponse(string message)
        : this(false, message, null) { }

    public Contato Contato { get; private set; }
}
