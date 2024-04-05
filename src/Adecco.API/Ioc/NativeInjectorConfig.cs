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

            app.UseWhen(context => context.Request.Path.StartsWithSegments("/"), appBuilder =>
            {
                appBuilder.UseCustomWelcomePage(
                    new CustomWelcomePageOptions
                    {
                        Title = @"<p><h3>Adecco Teste Api</h3></p>",
                        Body = @"<p class=""card-text"">Esta é uma página de produção</p>
                         <p class=""card-text"">Se você está vendo isso, a produção está funcionando!</p>",
                        Message = "API em execução"
                    }
                );
            });

            app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), appBuilder =>
            {
                appBuilder.Use(async (context, next) =>
                {
                    await next();
                });
            });
        }
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseSerilogRequestLogging();
        app.MapControllers();
    }
}
