using Adecco.Core.Interfaces.UseCases;

namespace Adecco.Application.UseCases.Clientes.Base;

public interface IGetByIdClienteUseCase : IUseCase<int, ClienteResponseDto>
{
}
