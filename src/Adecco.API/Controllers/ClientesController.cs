using Adecco.Core.Abstractions;
using Adecco.Core.Interfaces.Validations;

namespace Adecco.API.Controllers;

[Route("/api/v1/[controller]")]
public class ClientesController : Controller
{
    public ClientesController(IClienteService productService, IMapper mapper, IEnderecoService enderecoService, IContatoService contatoService, IValidacaoService validacaoService)
    {
        _clienteService = productService;
        _mapper = mapper;
        _enderecoService = enderecoService;
        _contatoService = contatoService;
        _validacaoService = validacaoService;
    }

    private readonly IClienteService _clienteService;
    private readonly IContatoService _contatoService;
    private readonly IEnderecoService _enderecoService;
    private readonly IValidacaoService _validacaoService;

    private readonly IMapper _mapper;

    [HttpGet("/cliente/listar")]
    public async Task<IEnumerable<ClienteResponseDto>> ListAsync(string? nome, string? email, string? cpf)
    {
        var clientes = await _clienteService.ListAsync(nome, email, cpf);
        var response = _mapper.Map<IEnumerable<Cliente>, IEnumerable<ClienteResponseDto>>(clientes);
        return response;
    }

    [HttpPost("/cliente/criar")]
    public async Task<IActionResult> PostAsync([FromBody] ClienteRequestDto request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
        var contato = _mapper.Map<ContatoRequestDto, Contato>(request.Contato);
        var endereco = _mapper.Map<EnderecoRequestDto, Endereco>(request.Endereco);
        var cliente = _mapper.Map<ClienteRequestDto, Cliente>(request);
        cliente.AdicionarContato(contato);
        cliente.AdicionarEndereco(endereco);
        var validacaoResponse = new CustomResponse();
        _validacaoService.Validar(cliente, _validacaoService.ValidarCliente, "Cliente", validacaoResponse);
        _validacaoService.Validar(new List<Contato> { contato }, _validacaoService.ValidarContato, "Contato", validacaoResponse);
        _validacaoService.Validar(new List<Endereco> { endereco }, _validacaoService.ValidarEndereco, "Endereço", validacaoResponse);
        if (!validacaoResponse.Success) return BadRequest(validacaoResponse);
        var enderecoResponse = await _enderecoService.SaveAsync(endereco);
        var contatoResponse = await _contatoService.SaveAsync(contato);
        if (!contatoResponse.Success) return BadRequest(contatoResponse.Message);
        if (!enderecoResponse.Success) return BadRequest(enderecoResponse.Message);
        var result = await _clienteService.SaveAsync(cliente);
        if (!result.Success) return BadRequest(result.Message);
        var response = _mapper.Map<Cliente, ClienteResponseDto>(result.Cliente);
        return Ok(response);
    }

    [HttpPut("/cliente/atualizar/{clienteId}")]
    public async Task<IActionResult> PutAsync(int clienteId, int enderecoId, int contatoId, [FromBody] ClienteRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.GetErrorMessages());
        }

        var clienteExistente = await _clienteService.FindByIdAsync(clienteId);
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

        var result = await _clienteService.UpdateAsync(clienteId, clienteExistente);
        if (!result.Success)
        {
            return BadRequest(result.Message);
        }

        var clienteResponse = _mapper.Map<Cliente, ClienteResponseDto>(result.Cliente);
        return Ok(clienteResponse);
    }

    [HttpDelete("/cliente/remover/{clienteId}")]
    public async Task<IActionResult> DeleteAsync(int clienteId)
    {
        var result = await _clienteService.DeleteAsync(clienteId);
        if (!result.Success)
        {
            return BadRequest(result.Message);
        }

        var response = _mapper.Map<Cliente, ClienteResponseDto>(result.Cliente);
        return Ok(response);
    }
}