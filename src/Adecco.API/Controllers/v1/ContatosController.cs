namespace Adecco.API.Controllers.v1;

[ApiVersion("1.0")]
public sealed class ContatosController : ControllerBase
{
    public ContatosController(
        IClienteJsonService clienteService,
        ILogger<ClientesController> logger
    )
    {
        _clienteService = clienteService;
        _logger = logger;
    }

    private readonly IClienteJsonService _clienteService;
    private readonly ILogger<ClientesController> _logger;

    [HttpPut("/atualizar/{id}/contato")]
    public IActionResult AtualizarContato(int id, [FromBody] Contato contato)
    {
        try
        {
            _clienteService.AtualizarContato(id, contato);
            return Ok();
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
    public IActionResult IncluirContato(int clienteId, [FromBody] Contato contato)
    {
        try
        {
            _clienteService.IncluirContato(clienteId, contato);
            return Ok();
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
