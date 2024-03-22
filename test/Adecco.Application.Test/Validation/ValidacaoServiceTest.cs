using Adecco.Core.Abstractions;
using Adecco.Core.Entities;
using Adecco.Core.Enums;

namespace Adecco.Application.Test.Validation;

public class ValidacaoServiceTest
{

    private readonly ValidacaoService _validacaoService;

    public ValidacaoServiceTest()
    {
        _validacaoService = new ValidacaoService();
    }

    [Theory]
    [InlineData("email@example.com", true)]
    [InlineData("emailincorrecto", false)]
    public void ValidarCampos_DeveValidarEmailCorretamente(string email, bool esperado)
    {
        var resultado = _validacaoService.ValidarEmail(email);
        resultado.IsValid.Should().Be(esperado);
    }

    [Theory]
    [InlineData("12345678909", true)]
    [InlineData("11111111111", false)]
    public void ValidarCampos_DeveValidarCPFCorretamente(string cpf, bool esperado)
    {
        var resultado = _validacaoService.ValidarCPF(cpf);
        resultado.IsValid.Should().Be(esperado);
    }

    [Theory]
    [InlineData("12345678", true)]
    [InlineData("87654321", true)]
    [InlineData("123", false)]
    public void ValidarCampos_DeveValidarCEPCorretamente(string cep, bool esperado)
    {
        var resultado = _validacaoService.ValidarCEP(cep);
        resultado.IsValid.Should().Be(esperado);
    }

    [Theory]
    [InlineData("1234567", true)]
    [InlineData("12A34B67", true)]
    [InlineData("1234", false)]
    [InlineData("12345678914", false)]
    public void ValidarRG_DeveValidarRGCorretamente(string rg, bool esperado)
    {
        var resultado = _validacaoService.ValidarRG(rg);
        resultado.IsValid.Should().Be(esperado);
    }

    [Theory]
    [InlineData(11,"999999999", ETipoContato.Celular, true)]
    [InlineData(12,"99999999", ETipoContato.Residencial, true)]
    [InlineData(25,"99999999", ETipoContato.Comercial, true)]
    [InlineData(31,"9999", ETipoContato.Celular, false)]
    public void ValidarTelefone_DeveValidarTelefoneCorretamente(int ddd, string telefone, ETipoContato tipoContato, bool esperado)
    {
        var resultado = _validacaoService.ValidarTelefone(telefone, tipoContato, ddd);
        resultado.IsValid.Should().Be(esperado);
    }

    [Fact]
    public void ValidarContato_DeveRetornarErroSeContatoForNulo()
    {
        Contato contato = null;
        var resultado = _validacaoService.ValidarContato(contato);
        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain("Contato é nulo.");
    }

    [Fact]
    public void ValidarEndereco_DeveRetornarErroSeEnderecoForNulo()
    {
        Endereco endereco = null;
        var resultado = _validacaoService.ValidarEndereco(endereco);
        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain("Endereço é nulo.");
    }


    [Theory]
    [InlineData(0, "", "12345678", "Rua Teste", "123", "Bairro", "Teste Complemento", "Cidade", "SP", "Perto da praça", ETipoEndereco.Cobranca, true)]
    [InlineData(0, "Endereco Sem ID", "", "", "", "", "", "", "", "", ETipoEndereco.Preferencial, false)]
    public void ValidarEndereco_DeveValidarEnderecoCorretamente(int id, string nome, string cep, string logradouro, string numero, string bairro, string complemento, string cidade, string estado, string referencia, ETipoEndereco tipoEndereco, bool esperado)
    {
        var endereco = new Endereco(id, nome, cep, logradouro, numero, bairro, complemento, cidade, estado, referencia, tipoEndereco);
        var resultado = _validacaoService.ValidarEndereco(endereco);
        resultado.IsValid.Should().Be(esperado);
    }


    [Fact]
    public void ValidarCliente_DeveRetornarErroSeClienteForNulo()
    {
        Cliente cliente = null;     
        var resultado = _validacaoService.ValidarCliente(cliente);
        resultado.IsValid.Should().BeFalse();      
        resultado.Errors.Should().Contain("Cliente é nulo.");          
    }


    [Theory]
    [InlineData(1, "Comercial", 0, 945564488, ETipoContato.Comercial, false)]
    [InlineData(3, "Financeiro", 11, 98469177, ETipoContato.Celular, false)]
    [InlineData(2, "COntabildade", 11, 984691772, ETipoContato.Celular, true)]
    [InlineData(4, "Marketing", 11, 44556633, ETipoContato.Residencial, true)]
    public void ValidarContato_DeveValidarContatoCorretamente(int id, string nome, int ddd, decimal telefone, ETipoContato tipoContato, bool esperado)
    {
        var contato = new Contato(id, nome, ddd, telefone, tipoContato);
        var resultado = _validacaoService.ValidarContato(contato);
        resultado.IsValid.Should().Be(esperado);
    }


    [Theory]
    [InlineData(0, false)]    
    [InlineData(11, true)]    
    public void ValidarDDD_DeveValidarCorretamente(int ddd, bool esperado)
    {
        var contato = new Contato(1, "Mariana ", ddd, 984691772, ETipoContato.Celular);
        var resultado = new CustomValidationResult();
        resultado.AddErrorIf(contato.DDD is < 11 or > 99, "DDD inválido.", "DDD");
        resultado.IsValid.Should().Be(esperado);
    }


    [Fact]
    public void ValidarCliente_DeveValidarClienteCorretamente()
    {
        var contatos = new List<Contato>
    {
        new Contato(id: 0, nome: "Contato 1", dDD: 11, telefone: 999999999M, tipoContato: ETipoContato.Celular)
    };

        var enderecos = new List<Endereco>
    {
        new Endereco(id: 0, nome: "Endereco 1", cep: "12345678", logradouro: "Rua Teste", numero: "123", bairro: "Bairro", complemento: "Complemento", cidade: "Cidade", estado: "SP", referencia: "Perto de algo", tipoEndereco: ETipoEndereco.Cobranca)
    };

        var cliente = new Cliente(id: 0, nome: "Cliente 1", email: "email@example.com", cpf: "12345678909", rg: "12A34B67", contatos: contatos, enderecos: enderecos);

        var resultado = _validacaoService.ValidarCliente(cliente);
        resultado.IsValid.Should().BeTrue();
    }


}
