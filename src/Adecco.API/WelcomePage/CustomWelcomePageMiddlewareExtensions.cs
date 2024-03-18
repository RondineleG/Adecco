namespace Adecco.API.WelcomePage;

public static class CustomWelcomePageMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomWelcomePage(
        this IApplicationBuilder app,
        CustomWelcomePageOptions options
    )
    {
        return app.UseMiddleware<CustomWelcomePageMiddleware>(options);
    }
}