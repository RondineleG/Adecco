using Adecco.API.Controllers.Base;
using Adecco.Application.Extensions;
using Adecco.Core.Enums;
using Adecco.Persistence.Extensions;
namespace Adecco.API.Controllers.v1;

[ApiVersion("1.0")]
public class PeopleController : BaseController
{
    [HttpGet]
    public IActionResult Get(string? nome, string? email, string? cpf)
    {
        var clientes = JsonFileHelper.LerArquivoJson();
        if (!string.IsNullOrWhiteSpace(nome))
        {
            clientes = clientes.Where(c => c.Nome.Contains(nome.Trim(), StringComparison.OrdinalIgnoreCase)).ToList();
        }
        if (!string.IsNullOrWhiteSpace(email))
        {
            clientes = clientes.Where(c => c.Email.Contains(email.Trim(), StringComparison.OrdinalIgnoreCase)).ToList();
        }
        if (!string.IsNullOrWhiteSpace(cpf))
        {
            clientes = clientes.Where(c => c.CPF.Contains(cpf.Trim(), StringComparison.OrdinalIgnoreCase)).ToList();
        }
        return Ok(clientes);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var people = JsonFileHelper.LerArquivoJson();
        var person = people.FirstOrDefault(p => p.Id == id);
        if (person == null)
        {
            return NotFound();
        }
        return Ok(person);
    }

    [HttpPost]
    public IActionResult Create([FromBody] ClienteRequestDto request)
    {
        var clientes = JsonFileHelper.LerArquivoJson();
        var novoCliente = ConverterRequestParaCliente(request, clientes);
        JsonFileHelper.WriteToJsonFile(clientes, JsonFileHelper.ArquivoJson());
        var responseDto = ConverterClienteParaResponseDto(novoCliente);
        return Ok(responseDto);
    }


    private ClienteResponseDto ConverterClienteParaResponseDto(Cliente cliente)
    {
        var clienteResponseDto = new ClienteResponseDto
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Email = cliente.Email,
            CPF = cliente.CPF,
            RG = cliente.RG,
            Contatos = cliente.Contatos.Select(contato => new ContatoResponseDto
            {
                Id = contato.Id,
                Nome = contato.Nome,
                DDD = contato.DDD,
                Telefone = contato.Telefone,
                TipoContato = contato.TipoContato.ToDescriptionString()
            }).ToList(),
            Enderecos = cliente.Enderecos.Select(endereco => new EnderecoResponseDto
            {
                Id = endereco.Id,
                CEP = endereco.CEP,
                Logradouro = endereco.Logradouro,
                Numero = endereco.Numero,
                Bairro = endereco.Bairro,
                Complemento = endereco.Complemento,
                Cidade = endereco.Cidade,
                Estado = endereco.Estado,
                Referencia = endereco.Referencia,
                TipoEndereco = endereco.TipoEndereco.ToDescriptionString()
            }).ToList()
        };

        return clienteResponseDto;
    }
    private Cliente ConverterRequestParaCliente(ClienteRequestDto request, List<ClienteResponseDto> clientesExistentes)
    {
        var maiorIdCliente = clientesExistentes.Any() ? clientesExistentes.Max(c => c.Id) : 0;
        var maiorIdContato = clientesExistentes.SelectMany(c => c.Contatos).Any() ? clientesExistentes.SelectMany(c => c.Contatos).Max(contato => contato.Id) : 0;
        var maiorIdEndereco = clientesExistentes.SelectMany(c => c.Enderecos).Any() ? clientesExistentes.SelectMany(c => c.Enderecos).Max(endereco => endereco.Id) : 0;
        var novoClienteId = maiorIdCliente + 1;
        var novoContatoId = maiorIdContato + 1;
        var novoEnderecoId = maiorIdEndereco + 1;
        var contato = new Contato(novoContatoId, request.Contato.Nome, request.Contato.DDD, request.Contato.Telefone, (ETipoContato)request.Contato.TipoContato);
        var endereco = new Endereco(novoEnderecoId, request.Endereco.Logradouro, request.Endereco.Numero, request.Endereco.Complemento, request.Endereco.Bairro, request.Endereco.Cidade, request.Endereco.Estado, request.Endereco.CEP, (ETipoEndereco)request.Endereco.TipoEndereco);
        var cliente = new Cliente(novoClienteId, request.Nome, request.Email, request.CPF, request.RG, new List<Contato> { contato }, new List<Endereco> { endereco });
        return cliente;
    }



    [HttpPut("{id}")]
    public ActionResult Update(int id, [FromBody] ClienteRequestDto updatedPerson)
    {
        var people = JsonFileHelper.LerArquivoJson();
        var person = people.FirstOrDefault(p => p.Id == id);

        if (person == null)
        {
            return NotFound();
        }
        JsonFileHelper.WriteToJsonFile(people);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var people = JsonFileHelper.LerArquivoJson();
        var person = people.FirstOrDefault(p => p.Id == id);

        if (person == null) return NotFound();

        people.Remove(person);
        JsonFileHelper.WriteToJsonFile(people);

        return NoContent();
    }

}