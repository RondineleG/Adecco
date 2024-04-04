namespace Adecco.API.Extensions;

public static class ServicesAndRepositoriesServicesExtensions
{
    public static void RegisterServicesAndRepositoriesServices(this IServiceCollection services)
    {
        services.AddScoped<IContatoRepository, CotatoRepository>();
        services.AddScoped<IClienteRepository, ClenteRepository>();
        services.AddScoped<IEnderecoRepository, EnderecoRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IContatoService, ContatoService>();
        services.AddScoped<IClienteService, ClienteService>();
        services.AddScoped<IEnderecoService, EnderecoService>();
        services.AddScoped<IValidacaoService, ValidacaoService>();
    }
}
