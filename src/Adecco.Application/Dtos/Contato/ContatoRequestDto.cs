namespace Adecco.Application.Dtos.Contato;

public sealed class ContatoRequestDto
{
    public ContatoRequestDto()
    {
        Nome = string.Empty;
        TipoContato = 3;
        DDD = 11;
        Telefone = decimal.MinValue;
    }

    [Required(ErrorMessage = "Campo {0} obrigat�rio")]
    [MaxLength(50)]
    public string Nome { get; set; }

    [Required(ErrorMessage = "Campo {0} obrigat�rio")]
    [Range(1, 3)]
    public int TipoContato { get; set; }

    [Required(ErrorMessage = "Campo {0} obrigat�rio")]
    [Range(11, 99)]
    public int DDD { get; set; }

    [Required(ErrorMessage = "Campo {0} obrigat�rio")]
    public decimal Telefone { get; set; }
}
