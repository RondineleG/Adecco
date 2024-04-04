namespace Adecco.API.Controllers.v2;

[ApiVersion("2.0")]
public sealed class ContatosController(
    IContatoService contatoService,
    IMapper mapper,
    IValidacaoService validacaoService
) : ApiBaseController
{
    private readonly IContatoService _contatoService = contatoService;
    private readonly IMapper _mapper = mapper;
    private readonly IValidacaoService _validacaoService = validacaoService;

    /// <summary>
    /// Retorna todos contatos cadastrados na base.
    /// </summary>
    /// <returns>Retorna os contatos encontrados</returns>
    /// <response code="200">Retorna os contatos encontrados</response>
    [CustomResponse(StatusCodes.Status200OK)]
    [CustomResponse(StatusCodes.Status404NotFound)]
    [CustomResponse(StatusCodes.Status500InternalServerError)]
    [HttpGet("/contato/listar")]
    public async Task<IActionResult> ListAsync(int? clienteId, int? contatoId)
    {
        var categories = await _contatoService.ListAsync(clienteId, contatoId);
        var resources = _mapper.Map<IEnumerable<Contato>, IEnumerable<ContatoResponseDto>>(
            categories
        );
        return ResponseOk(resources);
    }

    /// <summary>
    /// Cria um contato.
    /// </summary>
    /// <param name="clienteId">Id do cliente</param>
    /// <param name="request">Dados do contato</param>
    /// <returns>Um novo contato criado</returns>
    /// <response code="201">Retorna com o Id criado</response>
    /// <response code="400">Se o contato passado for nulo</response>
    /// <response code="500">Se houver um erro ao criar um contato</response>
    [CustomResponse(StatusCodes.Status201Created)]
    [CustomResponse(StatusCodes.Status400BadRequest)]
    [CustomResponse(StatusCodes.Status500InternalServerError)]
    [HttpPost("/contato/criar")]
    public async Task<IActionResult> PostAsync(int clienteId, [FromBody] ContatoRequestDto request)
    {
        if (!ModelState.IsValid)
            return ResponseBadRequest(ModelState.GetErrorMessages());
        var contato = _mapper.Map<ContatoRequestDto, Contato>(request);
        contato.AdicionarClienteId(clienteId);
        var validacaoResponse = new CustomResponse();
        _validacaoService.Validar(
            contato,
            _validacaoService.ValidarContato,
            "Contato",
            validacaoResponse
        );
        if (validacaoResponse.Status != CustomResultStatus.Success)
            return ResponseBadRequest(validacaoResponse);
        var result = await _contatoService.SaveAsync(contato);
        if (!result.Success)
            return ResponseBadRequest(result.Message);
        var contatoResponse = _mapper.Map<Contato, ContatoResponseDto>(result.Contato);
        return ResponseOk(contatoResponse);
    }

    /// <summary>
    /// Atualiza um contato.
    /// </summary>
    /// <param name="contatoId">Id do contato</param>
    ///<param name="request">Dados do contato</param>
    /// <response code="200">Retorna com o status da atualiza��o</response>
    /// <response code="400">Se o contato passado for nulo</response>
    /// <response code="500">Se houver um erro ao atualizar um contato</response>
    [CustomResponse(StatusCodes.Status200OK)]
    [CustomResponse(StatusCodes.Status400BadRequest)]
    [CustomResponse(StatusCodes.Status500InternalServerError)]
    [HttpPut("/contato/atualizar{contatoId}")]
    public async Task<IActionResult> PutAsync(int contatoId, [FromBody] ContatoRequestDto request)
    {
        if (!ModelState.IsValid)
            return ResponseBadRequest(ModelState.GetErrorMessages());
        var contato = _mapper.Map<ContatoRequestDto, Contato>(request);
        var result = await _contatoService.UpdateAsync(contatoId, contato);
        if (!result.Success)
            return ResponseBadRequest(result.Message);
        var contatoResponse = _mapper.Map<Contato, ContatoResponseDto>(result.Contato);
        return ResponseOk(contatoResponse);
    }

    /// <summary>
    /// Exclui um contato.
    /// </summary>
    /// <param name="contatoId">id do contato</param>
    /// <response code="200">Retorna com o status da exclus�o</response>
    /// <response code="400">Se o contato passado for nulo</response>
    /// <response code="500">Se houver um erro ao buscar um contato</response>
    [CustomResponse(StatusCodes.Status200OK)]
    [CustomResponse(StatusCodes.Status400BadRequest)]
    [CustomResponse(StatusCodes.Status500InternalServerError)]
    [HttpDelete("/contato/remover{contatoId}")]
    public async Task<IActionResult> DeleteAsync(int contatoId)
    {
        var result = await _contatoService.DeleteAsync(contatoId);
        if (!result.Success)
            return ResponseBadRequest(result.Message);
        var contatoResponse = _mapper.Map<Contato, ContatoResponseDto>(result.Contato);
        return ResponseOk(contatoResponse);
    }
}
