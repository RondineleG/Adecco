using Adecco.API.Ioc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.RegisterApplicationServices(builder.Configuration);

var app = builder.Build();
app.UseApplicationServices();
app.Run();