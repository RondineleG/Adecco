namespace Adecco.Application.Dtos.Cliente;

public class ClienteRequestDto
{
    [Required(ErrorMessage = "Campo {0} obrigatório")]
    [MaxLength(50)]
    public string Nome { get; set; }

    [Required]
    [MaxLength(30)]
    public string Email { get; set; }

    [Required(ErrorMessage = "Campo {0} obrigatório")]
    [StringLength(11)]
    public string CPF { get; set; }

    [Required(ErrorMessage = "Campo {0} obrigatório")]
    [StringLength(10)]
    public string RG { get; set; }

    [Required]
    public ContatoRequestDto Contato { get; set; }

    [Required]
    public EnderecoRequestDto Endereco { get; set; }
}