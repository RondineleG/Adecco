namespace Adecco.Application.Dtos.Cliente;

/// <summary>
/// DTO que representa os dados de resposta de um cliente.
/// </summary>
public sealed class ClienteResponseDto
{
    /// <summary>
    /// Construtor padr�o do DTO ClienteResponseDto.
    /// </summary>
    public ClienteResponseDto()
    {
        Nome = string.Empty;
        Email = string.Empty;
        CPF = string.Empty;
        RG = string.Empty;
        Contatos = new List<ContatoResponseDto>();
        Enderecos = new List<EnderecoResponseDto>();
    }

    /// <summary>
    /// Identificador �nico do cliente.
    /// </summary>
    /// <example>123</example>
    public int Id { get; set; }

    /// <summary>
    /// Nome do cliente.
    /// </summary>
    /// <example>Jo�o da Silva</example>
    public string Nome { get; set; }

    /// <summary>
    /// Endere�o de e-mail do cliente.
    /// </summary>
    /// <example>joao.silva@email.com</example>
    public string Email { get; set; }

    /// <summary>
    /// N�mero de CPF do cliente.
    /// </summary>
    /// <example>123.456.789-00</example>
    public string CPF { get; set; }

    /// <summary>
    /// N�mero de RG do cliente.
    /// </summary>
    /// <example>1234567</example>
    public string RG { get; set; }

    /// <summary>
    /// Lista de contatos associados ao cliente.
    /// </summary>
    /// <example>
    /// <code>
    /// new List<ContatoResponseDto>
    /// {
    ///     new ContatoResponseDto { Tipo = "Telefone", Valor = "(11) 98765-4321" },
    ///     new ContatoResponseDto { Tipo = "E-mail", Valor = "contato@email.com" }
    /// }
    /// </code>
    /// </example>
    public List<ContatoResponseDto> Contatos { get; set; }

    /// <summary>
    /// Lista de endere�os associados ao cliente.
    /// </summary>
    /// <example>
    /// <code>
    /// new List<EnderecoResponseDto>
    /// {
    ///     new EnderecoResponseDto { Logradouro = "Rua ABC", Numero = "123", Cidade = "S�o Paulo", CEP = "12345-678" },
    ///     new EnderecoResponseDto { Logradouro = "Av. XYZ", Numero = "456", Cidade = "Rio de Janeiro", CEP = "98765-432" }
    /// }
    /// </code>
    /// </example>
    public List<EnderecoResponseDto> Enderecos { get; set; }
}
