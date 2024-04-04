using Adecco.Application.AutoMapper;
using Adecco.Application.UseCases.Clientes.Base;

namespace Adecco.Application.UseCases.Clientes;

public class GetClienteByIdUseCase(IClienteService clienteService, IMapper mapper) : IGetByIdClienteUseCase
{
    private readonly IClienteService _clienteService = clienteService;
    private readonly IMapper _mapper = mapper;

    public async Task<CustomResult<ClienteResponseDto>> Execute(int clienteId, CancellationToken cancellationToken)
    {
        try
        {
            var clienteResult = await _clienteService.GetAsync(clienteId, cancellationToken);

            if (clienteResult.Status == CustomResultStatus.Success && clienteResult.Data != null)
            {
                var response = _mapper.Map<ClienteResponseDto>(clienteResult.Data);
                return CustomResult<ClienteResponseDto>.Success(response);
            }
            else
            {
                return clienteResult.MapCustomResult<Cliente, ClienteResponseDto>(_mapper);
            }
        }
        catch (Exception ex)
        {
            return CustomResult<ClienteResponseDto>.WithError(ex);
        }
    }

}