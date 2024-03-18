using Adecco.Core.Abstractions;
using Adecco.Core.Interfaces.Validations;

namespace Adecco.API.Controllers;

[Route("/api/v1/[controller]")]
public class EnderecosController : Controller
{
    public EnderecosController(IEnderecoService enderecoService, IMapper mapper, IValidacaoService validacaoService)
    {
        _enderecoService = enderecoService;
        _mapper = mapper;
        _validacaoService = validacaoService;
    }

    private readonly IEnderecoService _enderecoService;
    private readonly IMapper _mapper;
    private readonly IValidacaoService _validacaoService;

    [HttpGet("endereco/listar")]
    public async Task<IEnumerable<EnderecoResponseDto>> ListAsync()
    {
        var enderecos = await _enderecoService.ListAsync();
        var response = _mapper.Map<IEnumerable<Endereco>, IEnumerable<EnderecoResponseDto>>(enderecos);
        return response;
    }

    [HttpPost("endereco/criar")]
    public async Task<IActionResult> PostAsync(int clienteId, [FromBody] EnderecoRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.GetErrorMessages());
        }

        var endereco = _mapper.Map<EnderecoRequestDto, Endereco>(request);
        endereco.AdicionarClienteId(clienteId);
        var validacaoResponse = new CustomResponse();
        _validacaoService.Validar(endereco, _validacaoService.ValidarEndereco, "Endereco", validacaoResponse);
        if (!validacaoResponse.Success) return BadRequest(validacaoResponse);
        var result = await _enderecoService.SaveAsync(endereco);
        if (!result.Success)
        {
            return BadRequest(result.Message);
        }

        var response = _mapper.Map<Endereco, EnderecoResponseDto>(result.Endereco);
        return Ok(response);
    }

    [HttpPut("endereco/atualizar{enderecoId}")]
    public async Task<IActionResult> PutAsync(int enderecoId, [FromBody] EnderecoRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.GetErrorMessages());
        }

        var endereco = _mapper.Map<EnderecoRequestDto, Endereco>(request);
        var result = await _enderecoService.UpdateAsync(enderecoId, endereco);
        if (!result.Success)
        {
            return BadRequest(result.Message);
        }

        var contatoResponse = _mapper.Map<Endereco, EnderecoResponseDto>(result.Endereco);
        return Ok(contatoResponse);
    }

    [HttpDelete("endereco/remover{enderecoId}")]
    public async Task<IActionResult> DeleteAsync(int enderecoId)
    {
        var result = await _enderecoService.DeleteAsync(enderecoId);
        if (!result.Success)
        {
            return BadRequest(result.Message);
        }

        var contatoResponse = _mapper.Map<Endereco, EnderecoResponseDto>(result.Endereco);
        return Ok(contatoResponse);
    }
}