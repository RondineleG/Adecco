namespace Adecco.API.Extensions;

public static class LibrariesServicesExtensions
{
    public static void RegisterLibrariesServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MapperProfile));
    }
}
