namespace Adecco.Core.Abstractions;

public class EnderecoResponse : BaseResponse
{
    public EnderecoResponse(Endereco endereco)
        : this(true, string.Empty, endereco) { }

    public EnderecoResponse(string message)
        : this(false, message, null) { }

    private EnderecoResponse(bool success, string message, Endereco endereco)
        : base(success, message)
    {
        Endereco = endereco;
    }

    public Endereco Endereco { get; private set; }
}
