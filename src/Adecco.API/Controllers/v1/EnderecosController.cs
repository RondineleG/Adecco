using Adecco.Core.Abstractions;
using Adecco.Core.Interfaces.Validations;

namespace Adecco.API.Controllers.v1;

[ApiVersion("1.0")]
public sealed class EnderecosController : ControllerBase
{
    public EnderecosController(
        IClienteJsonService clienteService,
        ILogger<ClientesController> logger
,
        IMapper mapper,
        IValidacaoService validacaoService)
    {
        _clienteService = clienteService;
        _logger = logger;
        _mapper = mapper;
        _validacaoService = validacaoService;
    }

    private readonly IClienteJsonService _clienteService;
    private readonly ILogger<ClientesController> _logger;
    private readonly IMapper _mapper;
    private readonly IValidacaoService _validacaoService;

    [HttpPut("/atualizar/{clienteId}/endereco")]
    public async Task<IActionResult> AtualizarEndereco(int clienteId, [FromBody] EnderecoRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.GetErrorMessages());
        }
        try
        {
            var endereco = _mapper.Map<EnderecoRequestDto, Endereco>(request);
            var result = await _clienteService.AtualizarEndereco(clienteId, endereco);
            if (!result.Success) return BadRequest(result.Message);
            var contatoResponse = _mapper.Map<Endereco, EnderecoResponseDto>(result.Endereco);
            return Ok(contatoResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar endere�o do cliente.");
            return StatusCode(500, "Ocorreu um erro interno ao atualizar o endere�o do cliente.");
        }
    }

    [HttpDelete("/remover/{clienteId}/endereco/{enderecoId}")]
    public IActionResult RemoverEndereco(int clienteId, int enderecoId)
    {
        try
        {
            _clienteService.RemoverEndereco(clienteId, enderecoId);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover endere�o do cliente.");
            return StatusCode(500, "Ocorreu um erro interno ao remover o endere�o do cliente.");
        }
    }

    [HttpPost("{clienteId}/enderecos")]
    public async Task<IActionResult> IncluirEndereco(int clienteId, [FromBody] EnderecoRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.GetErrorMessages());
        }

        try
        {
            var endereco = _mapper.Map<EnderecoRequestDto, Endereco>(request);
            endereco.AdicionarClienteId(clienteId);
            var validacaoResponse = new CustomResponse();
            _validacaoService.Validar(
                endereco,
                _validacaoService.ValidarEndereco,
                "Endereco",
                validacaoResponse
            );
            if (!validacaoResponse.Success)
                return BadRequest(validacaoResponse);
            var result = await _clienteService.IncluirEndereco(clienteId, endereco);
            if (!result.Success) return BadRequest(result.Message);
            var response = _mapper.Map<Endereco, EnderecoResponseDto>(result.Endereco);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Erro ao incluir endereco no cliente com ID {ClienteId}",
                clienteId
            );
            return StatusCode(500, "Ocorreu um erro interno ao incluir o endereco.");
        }
    }
}