namespace Adecco.Core.Abstractions;

public sealed class ClienteResponse : BaseResponse
{
    public ClienteResponse() { }
    public ClienteResponse(Cliente cliente)
        : this(true, string.Empty, cliente) { }

    public ClienteResponse(string message)
        : this(false, message, null) { }

    private ClienteResponse(bool success, string message, Cliente cliente)
        : base(success, message)
    {
        Cliente = cliente;
    }

    public Cliente Cliente { get; private set; }
}