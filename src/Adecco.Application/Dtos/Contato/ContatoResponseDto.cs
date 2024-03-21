namespace Adecco.Application.Dtos.Contato;

public sealed class ContatoResponseDto
{
    public ContatoResponseDto()
    {
        Id = int.MinValue;
        Nome = string.Empty;
        TipoContato = string.Empty;
        DDD = 11;
        Telefone = decimal.MinValue;
    }
    public int Id { get; set; }

    public string Nome { get; set; }

    public string TipoContato { get; set; }

    public int DDD { get; set; }

    public decimal Telefone { get; set; }
}