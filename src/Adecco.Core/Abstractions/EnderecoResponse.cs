namespace Adecco.Core.Abstractions;

public sealed class EnderecoResponse : BaseResponse
{
    private EnderecoResponse(bool success, string message, Endereco endereco)
        : base(success, message)
    {
        Endereco = endereco;
    }

    public EnderecoResponse(Endereco endereco)
        : this(true, string.Empty, endereco) { }

    public EnderecoResponse(string message)
        : this(false, message, new Endereco()) { }

    public Endereco Endereco { get; private set; }
}