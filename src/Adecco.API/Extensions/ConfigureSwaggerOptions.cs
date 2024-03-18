namespace Adecco.API.Extensions;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

    public void Configure(SwaggerGenOptions options)
    {
        for (var i = 0; i < _provider.ApiVersionDescriptions.Count; i++)
        {
            var description = _provider.ApiVersionDescriptions[i];
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var info = new OpenApiInfo()
        {
            Title = "Adecco Teste Api",
            Version = description.ApiVersion.ToString(),
            Description = "Adecco Teste Api",
            Contact = new OpenApiContact
            {
                Name = "Rondinele Guimarães",
                Email = "rondineleg@gmail.com",
                Url = new Uri("https://www.linkedin.com/in/rondineleg")
            },
            License = new OpenApiLicense
            {
                Name = "MIT",
                Url = new Uri("https://opensource.org/licenses/MIT")
            }
        };

        if (description.IsDeprecated)
            info.Description += "Esta versão da API está obsoleta";

        return info;
    }
}
