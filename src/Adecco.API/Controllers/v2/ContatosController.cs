namespace Adecco.API.Controllers.v2;

[ApiVersion("2.0")]
public sealed class ContatosController(
    IContatoService contatoService,
    IMapper mapper,
    IValidacaoService validacaoService
) : BaseController
{
    private readonly IContatoService _contatoService = contatoService;
    private readonly IMapper _mapper = mapper;
    private readonly IValidacaoService _validacaoService = validacaoService;

    [HttpGet("/contato/listar")]
    public async Task<IEnumerable<ContatoResponseDto>> ListAsync(int? clienteId, int? contatoId)
    {
        var categories = await _contatoService.ListAsync(clienteId, contatoId);
        var resources = _mapper.Map<IEnumerable<Contato>, IEnumerable<ContatoResponseDto>>(
            categories
        );
        return resources;
    }

    [HttpPost("/contato/criar")]
    public async Task<IActionResult> PostAsync(int clienteId, [FromBody] ContatoRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());
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
        var result = await _contatoService.SaveAsync(contato);
        if (!result.Success)
            return BadRequest(result.Message);
        var contatoResponse = _mapper.Map<Contato, ContatoResponseDto>(result.Contato);
        return Ok(contatoResponse);
    }

    [HttpPut("/contato/atualizar{contatoId}")]
    public async Task<IActionResult> PutAsync(int contatoId, [FromBody] ContatoRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());
        var contato = _mapper.Map<ContatoRequestDto, Contato>(request);
        var result = await _contatoService.UpdateAsync(contatoId, contato);
        if (!result.Success)
            return BadRequest(result.Message);
        var contatoResponse = _mapper.Map<Contato, ContatoResponseDto>(result.Contato);
        return Ok(contatoResponse);
    }

    [HttpDelete("/contato/remover{contatoId}")]
    public async Task<IActionResult> DeleteAsync(int contatoId)
    {
        var result = await _contatoService.DeleteAsync(contatoId);
        if (!result.Success)
            return BadRequest(result.Message);
        var contatoResponse = _mapper.Map<Contato, ContatoResponseDto>(result.Contato);
        return Ok(contatoResponse);
    }
}
