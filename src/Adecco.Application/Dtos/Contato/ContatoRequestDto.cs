namespace Adecco.Application.Dtos.Contato;

public class ContatoRequestDto
{
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
