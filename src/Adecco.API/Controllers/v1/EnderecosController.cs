namespace Adecco.API.Controllers.v1;

[ApiVersion("1.0")]
public sealed class EnderecosController : ControllerBase
{
    public EnderecosController(
        IClienteJsonService clienteService,
        ILogger<ClientesController> logger
    )
    {
        _clienteService = clienteService;
        _logger = logger;
    }

    private readonly IClienteJsonService _clienteService;
    private readonly ILogger<ClientesController> _logger;

    [HttpPut("/atualizar/{id}/endereco")]
    public IActionResult AtualizarEndereco(int id, [FromBody] Endereco endereco)
    {
        try
        {
            _clienteService.AtualizarEndereco(id, endereco);
            return Ok();
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
    public IActionResult IncluirEndereco(int clienteId, [FromBody] Endereco endereco)
    {
        try
        {
            _clienteService.IncluirEndereco(clienteId, endereco);
            return Ok();
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
