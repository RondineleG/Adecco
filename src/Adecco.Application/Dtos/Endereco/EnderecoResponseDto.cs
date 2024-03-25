namespace Adecco.Application.Dtos.Endereco;

/// <summary>
/// DTO que representa os dados de resposta de um endereço.
/// </summary>
public sealed class EnderecoResponseDto
{
    /// <summary>
    /// Construtor padrão do DTO EnderecoResponseDto.
    /// </summary>
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

    /// <summary>
    /// Identificador único do endereço.
    /// </summary>
    /// <example>123</example>
    public int Id { get; set; }

    /// <summary>
    /// Nome do endereço.
    /// </summary>
    /// <example>Casa</example>
    public string Nome { get; set; }

    /// <summary>
    /// Tipo do endereço.
    /// </summary>
    /// <example>Residencial</example>
    public string TipoEndereco { get; set; }

    /// <summary>
    /// Código de Endereçamento Postal (CEP).
    /// </summary>
    /// <example>12345-678</example>
    public string CEP { get; set; }

    /// <summary>
    /// Nome do logradouro.
    /// </summary>
    /// <example>Rua ABC</example>
    public string Logradouro { get; set; }

    /// <summary>
    /// Número do endereço.
    /// </summary>
    /// <example>123</example>
    public string Numero { get; set; }

    /// <summary>
    /// Bairro do endereço.
    /// </summary>
    /// <example>Bairro A</example>
    public string Bairro { get; set; }

    /// <summary>
    /// Complemento do endereço.
    /// </summary>
    /// <example>Apto 101</example>
    public string Complemento { get; set; }

    /// <summary>
    /// Cidade do endereço.
    /// </summary>
    /// <example>São Paulo</example>
    public string Cidade { get; set; }

    /// <summary>
    /// Estado do endereço.
    /// </summary>
    /// <example>SP</example>
    public string Estado { get; set; }

    /// <summary>
    /// Ponto de referência do endereço.
    /// </summary>
    /// <example>Próximo à Praça Central</example>
    public string Referencia { get; set; }
}
