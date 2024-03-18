namespace Adecco.Application.Validation;

public sealed class ValidacaoService : IValidacaoService
{
    public CustomValidationResult ValidarEmail(string email)
    {
        return ValidarCampos(email, RegexPatterns.Email, "Email");
    }

    public CustomValidationResult ValidarCPF(string cpf)
    {
        cpf = cpf.Trim().Replace(".", "").Replace("-", "");
        var resultado = new CustomValidationResult();
        if (cpf.Length != 11 || cpf.All(c => c == cpf[0]) || !ValidarDigitosCPF(cpf))
        {
            resultado.AddError("CPF inválido.", "CPF");
        }
        return resultado;
    }

    public CustomValidationResult ValidarRG(string rg)
    {
        return ValidarCampos(rg, RegexPatterns.RG, "CEP");
    }

    public CustomValidationResult ValidarCEP(string cep)
    {
        return ValidarCampos(cep, RegexPatterns.CEP, "CEP");
    }

    public CustomValidationResult ValidarTelefone(string telefone, ETipoContato tipoContato)
    {
        var pattern = tipoContato switch
        {
            ETipoContato.Celular => RegexPatterns.Celular,
            ETipoContato.Residencial => RegexPatterns.ResidencialComercial,
            ETipoContato.Comercial => RegexPatterns.ResidencialComercial,
            _ => throw new ArgumentException("Tipo de contato inválido")
        };

        return ValidarCampos(telefone, pattern, "Telefone");
    }

    public CustomValidationResult ValidarContato(Contato contato)
    {
        var resultado = new CustomValidationResult();

        if (contato == null)
        {
            return resultado.AddError("Contato é nulo.");
        }

        resultado
            .AddErrorIf(contato.DDD is < 11 or > 99, "DDD inválido.", "DDD")
            .AddErrorIf(
                string.IsNullOrEmpty(contato.Telefone.ToString()),
                "Telefone é obrigatório.",
                "Telefone"
            );

        if (!string.IsNullOrEmpty(contato.Telefone.ToString()))
        {
            var tipoContato =
                (
                    contato.Telefone.ToString().StartsWith("9")
                    && contato.Telefone.ToString().Length == 9
                )
                    ? ETipoContato.Celular
                    : ETipoContato.Residencial;
            resultado = ValidarTelefone(contato.Telefone.ToString(), tipoContato);
        }

        return resultado;
    }

    public CustomValidationResult ValidarEndereco(Endereco endereco)
    {
        var resultado = new CustomValidationResult();

        if (endereco == null)
        {
            return resultado.AddError("Endereço é nulo.");
        }
        resultado
            .AddErrorIf(string.IsNullOrWhiteSpace(endereco.CEP), "CEP é obrigatório.", "CEP")
            .AddErrorIf(!Regex.IsMatch(endereco.CEP, RegexPatterns.CEP), "CEP inválido.", "CEP")
            .AddErrorIf(
                string.IsNullOrWhiteSpace(endereco.Logradouro),
                "Logradouro é obrigatório.",
                "Logradouro"
            )
            .AddErrorIf(string.IsNullOrWhiteSpace(endereco.Numero), "Número inválido.", "Numero")
            .AddErrorIf(
                string.IsNullOrWhiteSpace(endereco.Bairro),
                "Bairro é obrigatório.",
                "Bairro"
            )
            .AddErrorIf(
                string.IsNullOrWhiteSpace(endereco.Cidade),
                "Cidade é obrigatória.",
                "Cidade"
            )
            .AddErrorIf(
                string.IsNullOrWhiteSpace(endereco.Estado) || endereco.Estado.Length != 2,
                "Estado inválido.",
                "Estado"
            );
        return resultado;
    }

    public CustomValidationResult ValidarCliente(Cliente cliente)
    {
        var resultado = new CustomValidationResult();

        resultado = resultado.Merge(ValidarEmail(cliente.Email));
        resultado = resultado.Merge(ValidarCPF(cliente.CPF));
        resultado = resultado.Merge(ValidarRG(cliente.RG));
        foreach (var contato in cliente.Contatos)
        {
            resultado = resultado.Merge(ValidarContato(contato));
        }
        foreach (var endereco in cliente.Enderecos)
        {
            resultado = resultado.Merge(ValidarEndereco(endereco));
        }
        return resultado;
    }

    public CustomValidationResult ValidarCampos(string valor, string pattern, string nomeCampo)
    {
        var resultado = new CustomValidationResult();
        if (!Regex.IsMatch(valor, pattern))
            resultado.AddError($"{nomeCampo} inválido.", nomeCampo);
        return resultado;
    }

    public void Validar<T>(
        IEnumerable<T> entidades,
        Func<T, CustomValidationResult> funcValidacao,
        string nomeEntidade,
        CustomResponse response
    )
    {
        foreach (var entidade in entidades)
        {
            var resultado = funcValidacao(entidade);
            var propriedadeId = entidade.GetType().GetProperty("Id");
            var idValor =
                propriedadeId != null
                    ? propriedadeId.GetValue(entidade)?.ToString()
                    : "Desconhecido";
            AdicionarErroSeInvalido(resultado, $"{nomeEntidade} {idValor}", response);
        }
    }

    public void Validar<T>(
        T entidade,
        Func<T, CustomValidationResult> funcValidacao,
        string nomeEntidade,
        CustomResponse response
    )
    {
        var resultado = funcValidacao(entidade);
        AdicionarErroSeInvalido(resultado, $"{nomeEntidade}", response);
    }

    public void AdicionarErroSeInvalido(
        CustomValidationResult resultado,
        string contexto,
        CustomResponse response
    )
    {
        if (!resultado.IsValid)
        {
            foreach (var erro in resultado.Errors)
            {
                response.AddEntityError(contexto, erro);
            }
        }
    }

    private bool ValidarDigitosCPF(string cpf)
    {
        int[] multiplicadores1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] multiplicadores2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];

        var tempCpf = cpf.Substring(0, 9);
        var soma = 0;

        for (var i = 0; i < 9; i++)
        {
            soma += int.Parse(tempCpf[i].ToString()) * multiplicadores1[i];
        }

        var resto = soma % 11;
        if (resto < 2)
        {
            resto = 0;
        }
        else
        {
            resto = 11 - resto;
        }

        var digito = resto.ToString();
        tempCpf += digito;
        soma = 0;

        for (var i = 0; i < 10; i++)
        {
            soma += int.Parse(tempCpf[i].ToString()) * multiplicadores2[i];
        }

        resto = soma % 11;
        if (resto < 2)
        {
            resto = 0;
        }
        else
        {
            resto = 11 - resto;
        }

        digito += resto.ToString();

        return cpf.EndsWith(digito);
    }
}