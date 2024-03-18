using Adecco.Application.AutoMapper;
using Adecco.Core.Interfaces.Validations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connection = builder.Configuration["DefaultConnection:ConnectionString"];
builder.Services.AddDbContext<ApplicattionDataContext>(options => options.UseSqlite(connection));
builder.Services.AddScoped<IContatoRepository, CotatoRepository>();
builder.Services.AddScoped<IClienteRepository, ClenteRepository>();
builder.Services.AddScoped<IEnderecoRepository, EnderecoRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IContatoService, ContatoService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IEnderecoService, EnderecoService>();
builder.Services.AddScoped<IValidacaoService, ValidacaoService>();
builder.Services.AddAutoMapper(typeof(MapperProfile));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
    app.UseCustomWelcomePage(new CustomWelcomePageOptions { Message = $"API {app.Environment} em execução" });
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();