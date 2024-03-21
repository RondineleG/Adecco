namespace Adecco.Application.Dtos.Cliente;

public sealed class ClienteResponseDto
{
    public ClienteResponseDto()
    {
        Nome = string.Empty;
        Email = string.Empty;
        CPF = string.Empty;
        RG = string.Empty;
        Contatos = [];
        Enderecos = [];
    }

    public int Id { get; set; }

    public string Nome { get; set; }

    public string Email { get; set; }

    public string CPF { get; set; }

    public string RG { get; set; }

    public List<ContatoResponseDto> Contatos { get; set; }

    public List<EnderecoResponseDto> Enderecos { get; set; }
}