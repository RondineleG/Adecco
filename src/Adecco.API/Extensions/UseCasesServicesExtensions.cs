using Adecco.Application.UseCases.Clientes.Base;
using Adecco.Application.UseCases.Clientes;

namespace Adecco.API.Extensions;

public static class UseCasesServicesExtensions
{
    public static void RegisterUseCasesServices(this IServiceCollection services)
    {
        services.AddScoped<IGetAllClienteUseCase, GetAllClienteUseCase>();
        services.AddScoped<IGetByIdClienteUseCase, GetClienteByIdUseCase>();
    }
}
