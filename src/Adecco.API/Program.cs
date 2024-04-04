using Adecco.API.LogConfigurations;

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
var loggingSection = builder.Configuration.GetSection("Logging");
using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
       .AddConfiguration(loggingSection)
       .AddSimpleConsole()
       .AddConsoleFormatter<CsvLogFormatterConfiguration, ConsoleFormatterOptions>();
});

var logger = loggerFactory.CreateLogger<Program>();

builder.Services.RegisterApplicationServices(builder.Configuration);
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Host.UseSerilog();

var app = builder.Build();
app.UseApplicationServices();
try
{
    Log.Information("Iniciando o WebApi");
    logger.LogInformation("Iniciando o WebApi");


    using (var scope = logger.BeginScope("Scope 1"))
    {
        logger.LogInformation("Entrando  no Scope");
        Log.Warning("This is a Warning");
        logger.LogTrace("This is a trace message");
        logger.LogInformation("This is an information message in scope 1");
        logger.LogWarning("This is a warning message in scope 1");
        logger.LogError("This is an error message in scope 1");
        logger.LogCritical("This is a critical message in scope 1");
    }
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
