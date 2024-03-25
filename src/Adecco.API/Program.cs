var builder = WebApplication.CreateBuilder(args);

var hostEnvironment = builder.Environment;
builder
    .Configuration.SetBasePath(hostEnvironment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile(
        $"appsettings.{hostEnvironment.EnvironmentName}.json",
        optional: true,
        reloadOnChange: true
    )
    .AddEnvironmentVariables();

if (hostEnvironment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>(optional: true);
}
builder.Services.RegisterApplicationServices(builder.Configuration);
builder.Host.UseSerilog(
    (hostingContext, loggerConfiguration) =>
        loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration).CreateLogger()
);

var app = builder.Build();
app.UseApplicationServices();

try
{
    Log.Information("Iniciando o WebApi");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Erro catastrófico.");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
