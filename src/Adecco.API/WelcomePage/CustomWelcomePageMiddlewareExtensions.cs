namespace Adecco.API.WelcomePage;

public static class CustomWelcomePageMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomWelcomePage(
        this IApplicationBuilder app,
        CustomWelcomePageOptions options
    ) => app.UseMiddleware<CustomWelcomePageMiddleware>(options);
}
