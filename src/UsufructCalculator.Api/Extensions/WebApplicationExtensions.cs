using UsufructCalculator.Api.Middleware;

namespace UsufructCalculator.Api.Extensions;

/// <summary>
/// Extension methods for configuring the web application middleware pipeline.
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Configures the HTTP request pipeline with required middleware.
    /// </summary>
    /// <param name="app">The web application instance to configure.</param>
    /// <returns>The configured web application instance.</returns>
    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Don't do https redirection for development environments.
        if (!app.Environment.IsDevelopment())
        {
            app.UseHttpsRedirection();
        }

        app.UseCors("AllowAngularApp");

        // Add API Key authentication middleware
        app.UseMiddleware<ApiKeyAuthMiddleware>();

        app.MapControllers();

        // Map health check endpoint (no authentication required)
        app.MapHealthChecks("/health");

        return app;
    }
}
