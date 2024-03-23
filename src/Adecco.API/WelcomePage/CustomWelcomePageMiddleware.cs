namespace Adecco.API.WelcomePage;

public sealed class CustomWelcomePageMiddleware(
    RequestDelegate next,
    CustomWelcomePageOptions options,
    ILogger<CustomWelcomePageMiddleware> logger
)
{
    private readonly RequestDelegate _next = next;
    private readonly CustomWelcomePageOptions _options = options;
    private readonly ILogger<CustomWelcomePageMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.ContentType = "text/html";
        var page =
            $@"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Bem-vindo</title>
    <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css"" rel=""stylesheet"" integrity=""sha384-UG55wVHrS6K2u4l6LX6X7VyjtKr+Es7iMnd9bg5bsvMX3yxKKbrQSo0fJXDWmzrN"" crossorigin=""anonymous"">
</head>
<body>
    <div class=""container mt-5"">
        <div class=""card"">
            <div class=""card-header"">
                <h1 class=""card-title"">{_options.Title}</h1>
               <p class=""card-text"">{_options.Message}</p>
            </div>
            <div class=""card-body"">
             <div class=""card"">  {_options.Body}</p>
            </div>
        </div>
    </div>
    <script src=""https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js"" integrity=""sha384-rDp7OSyoj8R8XZkTf1f7rjy7CsbbVHH2KfWz9L4N6hP0WlRByeRxprWbqs+2lc4l"" crossorigin=""anonymous""></script>
</body>
</html>
";
        await context.Response.WriteAsync(page);
    }
}
