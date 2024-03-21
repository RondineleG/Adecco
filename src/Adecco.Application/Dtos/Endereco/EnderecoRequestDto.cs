namespace Adecco.Application.Dtos.Endereco;

public sealed class EnderecoRequestDto
{

    public EnderecoRequestDto()
    {
        Nome = string.Empty;
        TipoEndereco = 1;
        CEP = string.Empty;
        Logradouro = string.Empty;
        Numero = string.Empty;
        Bairro = string.Empty;
        Complemento = string.Empty;
        Cidade = string.Empty;
        Estado = string.Empty;
        Referencia = string.Empty;
    }

    [Required]
    [MaxLength(30)]
    public string Nome { get; set; }

    [Required(ErrorMessage = "Campo {0} obrigat�rio")]
    [Range(1, 3)]
    public int TipoEndereco { get; set; }

    [Required(ErrorMessage = "Campo {0} obrigat�rio")]
    [MaxLength(8)]
    [MinLength(8)]
    public string CEP { get; set; }

    [Required(ErrorMessage = "Campo {0} obrigat�rio")]
    [MaxLength(50)]
    public string Logradouro { get; set; }

    [Required(ErrorMessage = "Campo {0} obrigat�rio")]
    [MaxLength(50)]
    public string Numero { get; set; }

    [Required(ErrorMessage = "Campo {0} obrigat�rio")]
    [MaxLength(50)]
    public string Bairro { get; set; }

    [MaxLength(50)]
    public string Complemento { get; set; }

    [Required(ErrorMessage = "Campo {0} obrigat�rio")]
    [MaxLength(30)]
    public string Cidade { get; set; }

    [Required(ErrorMessage = "Campo {0} obrigat�rio")]
    [MaxLength(2)]
    public string Estado { get; set; }

    public string Referencia { get; set; }
}