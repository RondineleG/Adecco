namespace Adecco.Application.Dtos.Endereco;

/// <summary>
/// DTO que representa os dados de resposta de um endere�o.
/// </summary>
public sealed class EnderecoResponseDto
{
    /// <summary>
    /// Construtor padr�o do DTO EnderecoResponseDto.
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
    /// Identificador �nico do endere�o.
    /// </summary>
    /// <example>123</example>
    public int Id { get; set; }

    /// <summary>
    /// Nome do endere�o.
    /// </summary>
    /// <example>Casa</example>
    public string Nome { get; set; }

    /// <summary>
    /// Tipo do endere�o.
    /// </summary>
    /// <example>Residencial</example>
    public string TipoEndereco { get; set; }

    /// <summary>
    /// C�digo de Endere�amento Postal (CEP).
    /// </summary>
    /// <example>12345-678</example>
    public string CEP { get; set; }

    /// <summary>
    /// Nome do logradouro.
    /// </summary>
    /// <example>Rua ABC</example>
    public string Logradouro { get; set; }

    /// <summary>
    /// N�mero do endere�o.
    /// </summary>
    /// <example>123</example>
    public string Numero { get; set; }

    /// <summary>
    /// Bairro do endere�o.
    /// </summary>
    /// <example>Bairro A</example>
    public string Bairro { get; set; }

    /// <summary>
    /// Complemento do endere�o.
    /// </summary>
    /// <example>Apto 101</example>
    public string Complemento { get; set; }

    /// <summary>
    /// Cidade do endere�o.
    /// </summary>
    /// <example>S�o Paulo</example>
    public string Cidade { get; set; }

    /// <summary>
    /// Estado do endere�o.
    /// </summary>
    /// <example>SP</example>
    public string Estado { get; set; }

    /// <summary>
    /// Ponto de refer�ncia do endere�o.
    /// </summary>
    /// <example>Pr�ximo � Pra�a Central</example>
    public string Referencia { get; set; }
}
