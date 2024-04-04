using Adecco.API.Filters;

namespace Adecco.API.Ioc;

public static class NativeInjectorConfig
{
    public static void RegisterApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerAndConfigApiVersioning();
        services.AddAndConfigSwagger();
        services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));
        var connection = configuration["DefaultConnection:ConnectionString"];
        services.AddDbContext<EntityFrameworkDataContext>(options => options.UseSqlite(connection));
        services.RegisterUseCasesServices();
        services.RegisterServicesAndRepositoriesServices();
        services.RegisterLibrariesServices();

    }

    public static void UseApplicationServices(this WebApplication app)
    {
        app.UseExceptionHandler("/error");

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseCustomSwaggerUI();
        }
        if (app.Environment.IsProduction())
        {
            app.UseHsts();
            app.UseCustomWelcomePage(
                new CustomWelcomePageOptions
                {
                    Title =
                        @"<p>
                         <h3>Adecco Teste Api </h3>
                        </p>",
                    Body =
                        @"
                                <p class=""card-text"">Essa e uma pagina de produção</p>
                                <p class=""card-text"">Se esta vendo isso, prod esta funcionando!</p>
                                ",
                    Message = $"API em execução",
                }
            );
        }
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseSerilogRequestLogging();
        app.MapControllers();
    }
}
