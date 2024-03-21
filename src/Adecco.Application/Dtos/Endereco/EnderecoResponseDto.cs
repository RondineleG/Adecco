namespace Adecco.Application.Dtos.Endereco;

public sealed class EnderecoResponseDto
{
    public EnderecoResponseDto()
    {
        Id = int.MinValue;
        Nome = string.Empty;
        TipoEndereco = string.Empty;
        CEP = string.Empty;
        Logradouro = string.Empty;
        Numero = string.Empty;
        Bairro = string.Empty;
        Complemento = string.Empty;
        Cidade = string.Empty;
        Estado = string.Empty;
        Referencia = string.Empty;
    }
    public int Id { get; set; }

    public string Nome { get; set; }

    public string TipoEndereco { get; set; }

    public string CEP { get; set; }

    public string Logradouro { get; set; }

    public string Numero { get; set; }

    public string Bairro { get; set; }

    public string Complemento { get; set; }

    public string Cidade { get; set; }

    public string Estado { get; set; }
    public string Referencia { get; set; }
}