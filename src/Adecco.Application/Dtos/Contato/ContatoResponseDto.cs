namespace Adecco.Application.Dtos.Contato;

public sealed class ContatoResponseDto
{
    public int Id { get; set; }

    public string Nome { get; set; }

    public string TipoContato { get; set; }

    public int DDD { get; set; }

    public decimal Telefone { get; set; }
}
