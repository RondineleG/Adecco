using Adecco.Core.Abstractions;
using Adecco.Core.Interfaces.Validations;

namespace Adecco.API.Controllers.v1;

[ApiVersion("1.0")]
public sealed class ContatosController(
    IClienteJsonService clienteService,
    ILogger<ClientesController> logger
,
    IMapper mapper,
    IValidacaoService validacaoService) : ControllerBase
{
    private readonly IClienteJsonService _clienteService = clienteService;
    private readonly ILogger<ClientesController> _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly IValidacaoService _validacaoService = validacaoService;

    [HttpPut("/atualizar/{clienteId}/contato")]
    public async Task<IActionResult> AtualizarContato(int clienteId, [FromBody] ContatoRequestDto request)
    {
        if (!ModelState.IsValid) throw new BadRequestException(ModelState.GetErrorMessages());

        try
        {
            var contato = _mapper.Map<ContatoRequestDto, Contato>(request);
            contato.AdicionarClienteId(clienteId);
            var validacaoResponse = new CustomResponse();
            _validacaoService.Validar(contato, _validacaoService.ValidarContato, "Contato", validacaoResponse);
            if (!validacaoResponse.Success) throw new BadRequestException(validacaoResponse);
            var result = await _clienteService.AtualizarContato(clienteId, contato);
            if (!result.Success) throw new BadRequestException(result.Message);
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
        if (!ModelState.IsValid) throw new BadRequestException(ModelState.GetErrorMessages());

        try
        {
            var contato = _mapper.Map<ContatoRequestDto, Contato>(request);
            contato.AdicionarClienteId(clienteId);
            var validacaoResponse = new CustomResponse();
            _validacaoService.Validar(contato, _validacaoService.ValidarContato, "Contato", validacaoResponse);
            if (!validacaoResponse.Success) throw new BadRequestException(validacaoResponse);
            var result = await _clienteService.IncluirContato(clienteId, contato);
            if (!result.Success) throw new BadRequestException(result.Message);
            var contatoResponse = _mapper.Map<Contato, ContatoResponseDto>(result.Contato);
            return Ok(contatoResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao incluir contato no cliente com ID {ClienteId}", clienteId);
            return StatusCode(500, "Ocorreu um erro interno ao incluir o contato.");
        }
    }
}