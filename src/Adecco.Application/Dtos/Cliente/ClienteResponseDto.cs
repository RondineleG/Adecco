namespace Adecco.Application.Dtos.Cliente;

public sealed class ClienteResponseDto
{
    public ClienteResponseDto()
    {
        Contatos = new List<ContatoResponseDto>();
        Enderecos = new List<EnderecoResponseDto>();
    }

    public int Id { get; set; }

    public string Nome { get; set; }

    public string Email { get; set; }

    public string CPF { get; set; }

    public string RG { get; set; }

    public List<ContatoResponseDto> Contatos { get; private set; }

    public List<EnderecoResponseDto> Enderecos { get; private set; }
}
