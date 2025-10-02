using UsufructCalculator.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services
    .AddApiServices()
    .AddCorsPolicy()
    .AddSwaggerDocumentation();

var app = builder.Build();

// Configure middleware pipeline
app.ConfigureMiddleware();

app.Run();

/// <summary>
/// Program class made accessible for integration tests.
/// </summary>
public partial class Program
{
}
