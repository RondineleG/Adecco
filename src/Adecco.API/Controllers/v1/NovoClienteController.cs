
using Adecco.Persistence.Extensions;

namespace Adecco.API.Controllers.v1;
[Route("api/[controller]")]
[ApiController]
public class NovoClienteController(IMapper mapper) : ControllerBase
{
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public ActionResult<List<ClienteResponseDto>> GetPeople()
    {
        return JsonFileHelper.ReadFromJson<ClienteResponseDto>();
    }

    [HttpGet("{id}")]
    public ActionResult<ClienteResponseDto> GetPerson(int id)
    {
        var people = JsonFileHelper.ReadFromJson<ClienteResponseDto>();
        var person = people.FirstOrDefault(p => p.Id == id);
        if (person == null) return NotFound();
        return person;
    }

    [HttpPost]
    public ActionResult<Cliente> CreatePerson([FromBody] ClienteRequestDto request)
    {
        var people = JsonFileHelper.ReadFromJson<Cliente>();
        var cliente = _mapper.Map<ClienteRequestDto, Cliente>(request);
        people.Add(cliente);
        JsonFileHelper.WriteToJson(people);
        return CreatedAtAction(nameof(GetPerson), new { id = cliente.Id }, request);
    }

    [HttpPut("{clienteId}")]
    public ActionResult UpdatePerson(int clienteId, int contatoId, int enderecoId, [FromBody] ClienteRequestDto request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
        var clienteResponseDto = JsonFileHelper.ReadFromJson<ClienteResponseDto>();
        var clienteIndex = clienteResponseDto.FindIndex(p => p.Id == clienteId);
        var person = clienteResponseDto.FirstOrDefault(p => p.Id == clienteId);
        var options = new JsonSerializerOptions { WriteIndented = true, PropertyNameCaseInsensitive = true };
        var jsonString = JsonSerializer.Serialize(clienteResponseDto, options);
        if (clienteIndex == -1) return NotFound($"Cliente com ID {clienteId} não encontrado.");
        var clienteExistente = clienteResponseDto[clienteIndex];
        var contatoExistente = clienteExistente.Contatos.FirstOrDefault(p => p.Id == contatoId);
        var enderecoExistente = clienteExistente.Enderecos.FirstOrDefault(p => p.Id == enderecoId);
        var cliente = _mapper.Map<ClienteRequestDto, Cliente>(request);
        if (contatoExistente != null)
        {
            var novoContato = _mapper.Map<ContatoRequestDto, Contato>(request.Contato);
            cliente.AdicionarContato(novoContato);
        }
       
        if (enderecoExistente != null)
        {
            var novoEndereco = _mapper.Map<EnderecoRequestDto, Endereco>(request.Endereco);
            cliente.AdicionarEndereco(novoEndereco);
        }
     
        cliente.AtualizarCliente(cliente.Id, cliente.Nome, cliente.Email, cliente.CPF, cliente.RG);
        var clienteAtualizadoDto = _mapper.Map<Cliente, ClienteResponseDto>(cliente);
        clienteResponseDto[clienteIndex] = clienteAtualizadoDto;
        JsonFileHelper.WriteToJson(clienteResponseDto);
        return NoContent();
    }



    [HttpDelete("{id}")]
    public ActionResult DeletePerson(int id)
    {
        var people = JsonFileHelper.ReadFromJson<Cliente>();
        var person = people.FirstOrDefault(p => p.Id == id);
        if (person == null) return NotFound();
        people.Remove(person);
        JsonFileHelper.WriteToJson(people);
        return NoContent();
    }
}
