namespace Adecco.API.Controllers.v1;

[ApiVersion("1.0")]
public sealed class ClientesController(
    IClienteJsonService clienteService,
    ILogger<ClientesController> logger,
    IValidacaoService validacaoService,
    IMapper mapper
) : ApiBaseController
{
    private readonly IClienteJsonService _clienteService = clienteService;
    private readonly IValidacaoService _validacaoService = validacaoService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<ClientesController> _logger = logger;

    [HttpGet("{clienteId}")]
    public IActionResult Get(int clienteId)
    {
        var people = JsonFileHelper.LerArquivoJson();
        var person = people.FirstOrDefault(p => p.Id == clienteId);
        if (person == null)
            return ResponseNotFound("Cliente", clienteId);
        return Ok(person);
    }

    [HttpGet]
    public IActionResult Get(string nome = "", string email = "", string cpf = "")
    {
        throw new Exception();

        //var clientes = JsonFileHelper.LerArquivoJson();
        //if (!string.IsNullOrWhiteSpace(nome))
        //{
        //    clientes = clientes
        //        .Where(c => c.Nome.Contains(nome.Trim(), StringComparison.OrdinalIgnoreCase))
        //        .ToList();
        //}
        //if (!string.IsNullOrWhiteSpace(email))
        //{
        //    clientes = clientes
        //        .Where(c => c.Email.Contains(email.Trim(), StringComparison.OrdinalIgnoreCase))
        //        .ToList();
        //}
        //if (!string.IsNullOrWhiteSpace(cpf))
        //{
        //    clientes = clientes
        //        .Where(c => c.CPF.Contains(cpf.Trim(), StringComparison.OrdinalIgnoreCase))
        //        .ToList();
        //}
        //return Ok(clientes);
    }

    [HttpGet("/cliente/listar")]
    public async Task<IEnumerable<ClienteResponseDto>> ListAsync(
        string nome,
        string email,
        string cpf
    )
    {
        var clientes = await _clienteService.ListarClientes(
            nome?.Trim(),
            email?.Trim(),
            cpf?.Trim()
        );
        var clienteResponseDto = _mapper.Map<List<ClienteResponseDto>>(clientes);
        return clienteResponseDto;
    }

    [HttpPost("/cliente/criar")]
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
        try
        {
            await _clienteService.AdicionarCliente(cliente);
            endereco.AdicionarClienteId(cliente.Id);
            contato.AdicionarClienteId(cliente.Id);
            await _clienteService.IncluirEndereco(cliente.Id, endereco);
            await _clienteService.IncluirContato(cliente.Id, contato);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                $"{nameof(ClientesController)} => {nameof(_clienteService.AdicionarCliente)} => Erro ao adicionar cliente."
            );
            return StatusCode(500, $"Ocorreu um erro interno ao adicionar o cliente: {ex.Message}");
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
            return ResponseBadRequest(ModelState.GetErrorMessages());
        var clienteExistente = await _clienteService.BuscarClientePodId(clienteId);
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

        try
        {
            await _clienteService.AtualizarCliente(clienteId, clienteExistente);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                $"{nameof(ClientesController)} => {nameof(_clienteService.AtualizarCliente)} => Erro ao atualizar cliente."
            );
            return StatusCode(500, $"Ocorreu um erro interno ao atualizar o cliente: {ex.Message}");
        }
    }

    [HttpDelete("/cliente/remover/{id}")]
    public IActionResult Remover(int id)
    {
        try
        {
            _clienteService.RemoverCliente(id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                $"{nameof(ClientesController)} => {nameof(_clienteService.RemoverCliente)} => Erro ao remover cliente."
            );
            return StatusCode(500, $"Ocorreu um erro interno ao remover o cliente: {ex.Message}");
        }
    }
}
