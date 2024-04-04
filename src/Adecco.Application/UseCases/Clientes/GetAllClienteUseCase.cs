using Adecco.Application.UseCases.Clientes.Base;

namespace Adecco.Application.UseCases.Clientes;

public class GetAllClienteUseCase(IClienteService clienteService, IMapper mapper) : IGetAllClienteUseCase
{
    private readonly IClienteService _clienteService = clienteService;
    private readonly IMapper _mapper = mapper;
    public async Task<CustomResult<ClienteResponseDto>> Execute(ClienteRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            var cliente = _mapper.Map<Cliente>(request);
            var clienteCriado = await _clienteService.GetAsync(cliente.Id, cancellationToken);
            var response = _mapper.Map<ClienteResponseDto>(clienteCriado);
            return CustomResult<ClienteResponseDto>.Success(response);
        }
        catch (Exception ex)
        {
            return CustomResult<ClienteResponseDto>.WithError(ex);
        }
    }

}