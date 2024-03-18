using Adecco.Core.Abstractions;
using Adecco.Core.Interfaces.Validations;

namespace Adecco.API.Controllers.v1;

[ApiVersion("1.0")]
public sealed class ContatosController : ControllerBase
{
    public ContatosController(
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

    [HttpPut("/atualizar/{clienteId}/contato")]
    public async Task<IActionResult> AtualizarContato(int clienteId, [FromBody] ContatoRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.GetErrorMessages());
        }
        try
        {
            var contato = _mapper.Map<ContatoRequestDto, Contato>(request);
            contato.AdicionarClienteId(clienteId);
            var validacaoResponse = new CustomResponse();
            _validacaoService.Validar(
                contato,
                _validacaoService.ValidarContato,
                "Contato",
                validacaoResponse
            );
            if (!validacaoResponse.Success)
                return BadRequest(validacaoResponse);
            var result = await _clienteService.AtualizarContato(clienteId, contato);
            if (!result.Success) return BadRequest(result.Message);
            var contatoResponse = _mapper.Map<Contato, ContatoResponseDto>(result.Contato);
            return Ok(contatoResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar contato do cliente.");
            return StatusCode(500, "Ocorreu um erro interno ao atualizar o contato do cliente.");
        }
    }

    [HttpDelete("/remover/{clienteId}/contato/{contatoId}")]
    public IActionResult RemoverContato(int clienteId, int contatoId)
    {
        try
        {
            _clienteService.RemoverContato(clienteId, contatoId);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover contato do cliente.");
            return StatusCode(500, "Ocorreu um erro interno ao remover o contato do cliente.");
        }
    }

    [HttpPost("/{clienteId}/contatos")]
    public async Task<IActionResult> IncluirContato(int clienteId, [FromBody] ContatoRequestDto request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }
            var contato = _mapper.Map<ContatoRequestDto, Contato>(request);
            contato.AdicionarClienteId(clienteId);
            var validacaoResponse = new CustomResponse();
            _validacaoService.Validar(
                contato,
                _validacaoService.ValidarContato,
                "Contato",
                validacaoResponse
            );
            if (!validacaoResponse.Success) return BadRequest(validacaoResponse);
            var result = await _clienteService.IncluirContato(clienteId, contato);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            var contatoResponse = _mapper.Map<Contato, ContatoResponseDto>(result.Contato);
            return Ok(contatoResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Erro ao incluir contato no cliente com ID {ClienteId}",
                clienteId
            );
            return StatusCode(500, "Ocorreu um erro interno ao incluir o contato.");
        }
    }
}