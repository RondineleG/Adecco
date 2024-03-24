namespace Adecco.API.Controllers.v2;

[ApiVersion("2.0")]
public sealed class EnderecosController(
    IEnderecoService enderecoService,
    IMapper mapper,
    IValidacaoService validacaoService
) : BaseController
{
    private readonly IEnderecoService _enderecoService = enderecoService;
    private readonly IMapper _mapper = mapper;
    private readonly IValidacaoService _validacaoService = validacaoService;

    /// <summary>
    /// Retorna todos contatos cadastrados na base.
    /// </summary>
    /// <param name="clienteId">Id do cliente</param>
    /// <param name="enderecoId">Id do endereco</param>
    /// <returns>Retorna os contatos encontrados</returns>
    /// <response code="200">Retorna os contatos encontrados</response>
    [CustomResponse(StatusCodes.Status200OK)]
    [CustomResponse(StatusCodes.Status404NotFound)]
    [CustomResponse(StatusCodes.Status500InternalServerError)]
    [HttpGet("endereco/listar")]
    public async Task<IActionResult> ListAsync(int? clienteId, int? enderecoId)
    {
        var enderecos = await _enderecoService.ListAsync(clienteId, enderecoId);
        var response = _mapper.Map<IEnumerable<Endereco>, IEnumerable<EnderecoResponseDto>>(
            enderecos
        );
        return ResponseOk(response);
    }

    /// <summary>
    /// Cria um endereco.
    /// </summary>
    /// <param name="clienteId">Id do endereco</param>
    /// <returns>Um novo endereco criado</returns>
    /// <response code="201">Retorna com o Id criado</response>
    /// <response code="400">Se o endereco passado for nulo</response>
    /// <response code="500">Se houver um erro ao criar um endereco</response>
    [CustomResponse(StatusCodes.Status201Created)]
    [CustomResponse(StatusCodes.Status400BadRequest)]
    [CustomResponse(StatusCodes.Status500InternalServerError)]
    [HttpPost("endereco/criar")]
    public async Task<IActionResult> PostAsync(int clienteId, [FromBody] EnderecoRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());
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
        var result = await _enderecoService.SaveAsync(endereco);
        if (!result.Success)
            return BadRequest(result.Message);
        var response = _mapper.Map<Endereco, EnderecoResponseDto>(result.Endereco);
        return ResponseOk(response);
    }

    /// <summary>
    /// Atualiza um endereco.
    /// </summary>
    /// <param name="enderecoId">Id do endereco</param>
    /// <response code="200">Retorna com o status da atualização</response>
    /// <response code="400">Se o endereco passado for nulo</response>
    /// <response code="500">Se houver um erro ao atualizar um endereco</response>
    [CustomResponse(StatusCodes.Status200OK)]
    [CustomResponse(StatusCodes.Status400BadRequest)]
    [CustomResponse(StatusCodes.Status500InternalServerError)]
    [HttpPut("endereco/atualizar{enderecoId}")]
    public async Task<IActionResult> PutAsync(int enderecoId, [FromBody] EnderecoRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrorMessages());
        var endereco = _mapper.Map<EnderecoRequestDto, Endereco>(request);
        var result = await _enderecoService.UpdateAsync(enderecoId, endereco);
        if (!result.Success)
            return BadRequest(result.Message);
        var contatoResponse = _mapper.Map<Endereco, EnderecoResponseDto>(result.Endereco);
        return ResponseOk(contatoResponse);
    }

    /// <summary>
    /// Exclui um endereco.
    /// </summary>
    /// <param name="enderecoId">id do endereco</param>
    /// <response code="200">Retorna com o status da exclusão</response>
    /// <response code="400">Se o endereco passado for nulo</response>
    /// <response code="500">Se houver um erro ao buscar um endereco</response>
    [CustomResponse(StatusCodes.Status200OK)]
    [CustomResponse(StatusCodes.Status400BadRequest)]
    [CustomResponse(StatusCodes.Status500InternalServerError)]
    [HttpDelete("endereco/remover{enderecoId}")]
    public async Task<IActionResult> DeleteAsync(int enderecoId)
    {
        var result = await _enderecoService.DeleteAsync(enderecoId);
        if (!result.Success)
            return BadRequest(result.Message);
        var contatoResponse = _mapper.Map<Endereco, EnderecoResponseDto>(result.Endereco);
        return ResponseOk(contatoResponse);
    }
}
