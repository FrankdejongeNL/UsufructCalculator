namespace UsufructCalculator.Api.Middleware;

/// <summary>
/// Middleware that validates API key authentication for incoming requests.
/// </summary>
public class ApiKeyAuthMiddleware
{
    private const string ApiKeyHeaderName = "X-API-Key";
    private readonly RequestDelegate _next;
    private readonly string _apiKey;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiKeyAuthMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="configuration">The application configuration containing the API key.</param>
    /// <exception cref="InvalidOperationException">Thrown when ApiKey is not configured or is empty.</exception>
    public ApiKeyAuthMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;

        _apiKey = configuration.GetValue<string>("ApiKey")
            ?? throw new InvalidOperationException(
                "ApiKey must be configured in appsettings.json. Add a non-empty 'ApiKey' value to your configuration.");

        if (string.IsNullOrWhiteSpace(_apiKey))
        {
            throw new InvalidOperationException(
                "ApiKey cannot be empty. Provide a valid API key in appsettings.json.");
        }
    }

    /// <summary>
    /// Invokes the middleware to validate the API key in the request headers.
    /// </summary>
    /// <param name="context">The HTTP context for the current request.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        // Skip authentication for Swagger in development and health checks
        if (context.Request.Path.StartsWithSegments("/swagger") ||
            context.Request.Path.StartsWithSegments("/health"))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key missing");
            return;
        }

        if (!string.Equals(_apiKey, extractedApiKey, StringComparison.Ordinal))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid API Key");
            return;
        }

        await _next(context);
    }
}
