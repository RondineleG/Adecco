using Adecco.API.Controllers.Base;
using Adecco.Core.Abstractions;
using Adecco.Core.Interfaces.Validations;

namespace Adecco.API.Controllers.v1;

[ApiVersion("1.0")]
public sealed class ClientesController : BaseController
{
    public ClientesController(
        IClienteJsonService clienteService,
        ILogger<ClientesController> logger,
        IValidacaoService validacaoService,
        IMapper mapper
    )
    {
        _clienteService = clienteService;
        _logger = logger;
        _validacaoService = validacaoService;
        _mapper = mapper;
    }

    private readonly IClienteJsonService _clienteService;
    private readonly IValidacaoService _validacaoService;
    private readonly IMapper _mapper;
    private readonly ILogger<ClientesController> _logger;

    [HttpGet("/cliente/listar")]
    public async Task<IEnumerable<ClienteResponseDto>> ListAsync(
        string nome,
        string email,
        string cpf
    )
    {
        var clientes = _clienteService.ListarClientes(nome?.Trim(), email?.Trim(), cpf?.Trim());
        var response = _mapper.Map<IEnumerable<Cliente>, IEnumerable<ClienteResponseDto>>(clientes);
        return response;
    }

    [HttpPost("/cliente/criar")]
    public async Task<IActionResult> PostAsync([FromBody] ClienteRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());
        var contato = _mapper.Map<ContatoRequestDto, Contato>(request.Contato);
        var endereco = _mapper.Map<EnderecoRequestDto, Endereco>(request.Endereco);
        var cliente = _mapper.Map<ClienteRequestDto, Cliente>(request);
        cliente.AdicionarContato(contato);
        cliente.AdicionarEndereco(endereco);
        var validacaoResponse = new CustomResponse();
        _validacaoService.Validar(
            cliente,
            _validacaoService.ValidarCliente,
            "Cliente",
            validacaoResponse
        );
        _validacaoService.Validar(
            new List<Contato> { contato },
            _validacaoService.ValidarContato,
            "Contato",
            validacaoResponse
        );
        _validacaoService.Validar(
            new List<Endereco> { endereco },
            _validacaoService.ValidarEndereco,
            "Endereco",
            validacaoResponse
        );
        if (!validacaoResponse.Success)
            return BadRequest(validacaoResponse);
        _clienteService.AdicionarCliente(cliente);
        _clienteService.IncluirEndereco(cliente.Id, endereco);
        _clienteService.IncluirContato(cliente.Id, contato);
        return Ok();
    }

    [HttpPut("/atualizar/{id}")]
    public IActionResult Atualizar(int id, Cliente clienteAtualizado)
    {
        try
        {
            _clienteService.AtualizarCliente(id, clienteAtualizado);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar cliente.");
            return StatusCode(500, "Ocorreu um erro interno ao atualizar o cliente.");
        }
    }

    [HttpPut("/cliente/atualizar/{clienteId}")]
    public async Task<IActionResult> PutAsync(
        int clienteId,
        int enderecoId,
        int contatoId,
        [FromBody] ClienteRequestDto request
    )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.GetErrorMessages());
        }

        var clienteExistente = _clienteService.BuscarClientePodId(clienteId);
        if (clienteExistente == null)
        {
            return NotFound($"Cliente com ID {clienteId} não encontrado.");
        }

        _mapper.Map(request, clienteExistente);

        var contatoExistente = clienteExistente.Contatos.FirstOrDefault(c => c.Id == contatoId);
        if (contatoExistente != null)
        {
            _mapper.Map(request.Contato, contatoExistente);
        }
        else
        {
            var novoContato = _mapper.Map<ContatoRequestDto, Contato>(request.Contato);
            clienteExistente.AdicionarContato(novoContato);
        }

        var enderecoExistente = clienteExistente.Enderecos.FirstOrDefault(e => e.Id == enderecoId);
        if (enderecoExistente != null)
        {
            _mapper.Map(request.Endereco, enderecoExistente);
        }
        else
        {
            var novoEndereco = _mapper.Map<EnderecoRequestDto, Endereco>(request.Endereco);
            clienteExistente.AdicionarEndereco(novoEndereco);
        }
        _clienteService.AtualizarCliente(clienteId, clienteExistente);
        return Ok();
    }

    [HttpDelete("/remover/{id}")]
    public IActionResult Remover(int id)
    {
        try
        {
            _clienteService.RemoverCliente(id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover cliente.");
            return StatusCode(500, "Ocorreu um erro interno ao remover o cliente.");
        }
    }
}
