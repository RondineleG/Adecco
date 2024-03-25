namespace Adecco.API.Controllers.v2;

[ApiVersion("2.0")]
public sealed class ClientesController(
    IClienteService productService,
    IMapper mapper,
    IEnderecoService enderecoService,
    IContatoService contatoService,
    IValidacaoService validacaoService
) : ApiBaseController
{
    private readonly IClienteService _clienteService = productService;
    private readonly IContatoService _contatoService = contatoService;
    private readonly IEnderecoService _enderecoService = enderecoService;
    private readonly IValidacaoService _validacaoService = validacaoService;
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Retorna todos clientes cadastrados na base.
    /// </summary>
    /// <returns>Retorna os clientes encontrados</returns>
    /// <response code="200">Retorna os clientes encontrados</response>
    [CustomResponse(StatusCodes.Status200OK)]
    [CustomResponse(StatusCodes.Status404NotFound)]
    [CustomResponse(StatusCodes.Status500InternalServerError)]
    [HttpGet("cliente/listar")]
    public async Task<IActionResult> ListAsync(string nome, string email, string cpf)
    {
        var clientes = await _clienteService.ListAsync(nome?.Trim(), email?.Trim(), cpf?.Trim());
        var response = _mapper.Map<IEnumerable<Cliente>, IEnumerable<ClienteResponseDto>>(clientes);
        return ResponseOk(response);
    }

    /// <summary>
    /// Cria um cliente.
    /// </summary>
    /// <param name="request">Dados do cliente</param>
    /// <returns>Um novo cliente criado</returns>
    /// <response code="201">Retorna com o Id criado</response>
    /// <response code="400">Se o cliente passado for nulo</response>
    /// <response code="500">Se houver um erro ao criar um cliente</response>
    [CustomResponse(StatusCodes.Status201Created)]
    [CustomResponse(StatusCodes.Status400BadRequest)]
    [CustomResponse(StatusCodes.Status500InternalServerError)]
    [HttpPost("criar")]
    public async Task<IActionResult> PostAsync([FromBody] ClienteRequestDto request)
    {
        if (!ModelState.IsValid)
            return ResponseBadRequest(ModelState.GetErrorMessages());
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
            return ResponseBadRequest(validacaoResponse);
        var enderecoResponse = await _enderecoService.SaveAsync(endereco);
        var contatoResponse = await _contatoService.SaveAsync(contato);
        if (!contatoResponse.Success)
            return ResponseBadRequest(contatoResponse.Message);
        if (!enderecoResponse.Success)
            return ResponseBadRequest(enderecoResponse.Message);
        var result = await _clienteService.SaveAsync(cliente);
        if (!result.Success)
            return ResponseBadRequest(result.Message);
        var response = _mapper.Map<Cliente, ClienteResponseDto>(result.Cliente);
        return ResponseOk(response);
    }

    /// <summary>
    /// Atualiza um cliente.
    /// </summary>
    /// <param name="clienteId">Id do cliente</param>
    /// <param name="enderecoId">Id do endereco</param>
    /// <param name="contatoId">Id do contato</param>
    /// <param name="request">Dados do contato</param>
    /// <response code="200">Retorna com o status da atualizacao</response>
    /// <response code="400">Se o cliente passado for nulo</response>
    /// <response code="500">Se houver um erro ao atualizar um cliente</response>
    [CustomResponse(StatusCodes.Status200OK)]
    [CustomResponse(StatusCodes.Status400BadRequest)]
    [CustomResponse(StatusCodes.Status500InternalServerError)]
    [HttpPut("atualizar/{clienteId}")]
    public async Task<IActionResult> PutAsync(
        int clienteId,
        int enderecoId,
        int contatoId,
        [FromBody] ClienteRequestDto request
    )
    {
        if (!ModelState.IsValid)
            return ResponseBadRequest(ModelState.GetErrorMessages());
        var clienteExistente = await _clienteService.FindByIdAsync(clienteId);
        if (clienteExistente == null)
            return ResponseNotFound("Cliente", clienteId);
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
            return ResponseBadRequest(result.Message);
        var clienteResponse = _mapper.Map<Cliente, ClienteResponseDto>(result.Cliente);
        return ResponseOk(clienteResponse);
    }

    /// <summary>
    /// Exclui um cliente.
    /// </summary>
    /// <param name="clienteId">id do cliente</param>
    /// <response code="200">Retorna com o status da exclus�o</response>
    /// <response code="400">Se o cliente passado for nulo</response>
    /// <response code="500">Se houver um erro ao cliente um livro</response>
    [CustomResponse(StatusCodes.Status200OK)]
    [CustomResponse(StatusCodes.Status400BadRequest)]
    [CustomResponse(StatusCodes.Status500InternalServerError)]
    [HttpDelete("remover/{clienteId}")]
    public async Task<IActionResult> DeleteAsync(int clienteId)
    {
        var result = await _clienteService.DeleteAsync(clienteId);
        if (!ModelState.IsValid)
            return ResponseBadRequest(ModelState.GetErrorMessages());
        var response = _mapper.Map<Cliente, ClienteResponseDto>(result.Cliente);
        return ResponseOk(response);
    }
}
