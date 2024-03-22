namespace Adecco.Application.Dtos.Cliente;

public sealed class ClienteRequestDto
{
    public ClienteRequestDto()
    {
        Nome = string.Empty;
        Email = string.Empty;
        CPF = string.Empty;
        RG = string.Empty;
        Contato = new ContatoRequestDto();
        Endereco = new EnderecoRequestDto();
    }

    [Required(ErrorMessage = "Campo {0} obrigat�rio")]
    [MaxLength(50)]
    public string Nome { get; set; }

    [Required]
    [MaxLength(30)]
    public string Email { get; set; }

    [Required(ErrorMessage = "Campo {0} obrigat�rio")]
    [StringLength(11)]
    public string CPF { get; set; }

    [Required(ErrorMessage = "Campo {0} obrigat�rio")]
    [StringLength(10)]
    public string RG { get; set; }

    [Required]
    public ContatoRequestDto Contato { get; set; }

    [Required]
    public EnderecoRequestDto Endereco { get; set; }
}
