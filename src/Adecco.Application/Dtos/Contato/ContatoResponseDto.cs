namespace Adecco.Application.Dtos.Contato;

/// <summary>
/// DTO que representa os dados de resposta de um contato.
/// </summary>
public sealed class ContatoResponseDto
{
    /// <summary>
    /// Construtor padrão do DTO ContatoResponseDto.
    /// </summary>
    public ContatoResponseDto()
    {
        Id = int.MinValue;
        Nome = string.Empty;
        TipoContato = string.Empty;
        DDD = 11;
        Telefone = decimal.MinValue;
    }

    /// <summary>
    /// Identificador único do contato.
    /// </summary>
    /// <example>123</example>
    public int Id { get; set; }

    /// <summary>
    /// Nome do contato.
    /// </summary>
    /// <example>João</example>
    public string Nome { get; set; }

    /// <summary>
    /// Tipo do contato.
    /// </summary>
    /// <example>Telefone</example>
    public string TipoContato { get; set; }

    /// <summary>
    /// Código de Discagem Direta (DDD) do telefone.
    /// </summary>
    /// <example>11</example>
    public int DDD { get; set; }

    /// <summary>
    /// Número de telefone.
    /// </summary>
    /// <example>987654321</example>
    public decimal Telefone { get; set; }
}
