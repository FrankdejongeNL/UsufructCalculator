using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using UsufructCalculator.Api.Models;
using UsufructCalculator.Api.Models.Enums;

namespace UsufructCalculator.Api.Tests.Integration;

/// <summary>
/// Integration tests for the Usufruct Calculator API
/// </summary>
public class UsufructCalculatorApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public UsufructCalculatorApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("X-API-Key", "dev-api-key-replace-in-production");
    }

    [Fact]
    public async Task GetUsufructCalculation_WithValidParameters_ReturnsOk()
    {
        // Arrange
        var queryString = "?amount=250000&age=65&gender=Male&calculationMethod=EenLeven";

        // Act
        var response = await _client.GetAsync($"/api/v1/usufruct-calculations{queryString}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetUsufructCalculation_WithValidParameters_ReturnsUsufructResponse()
    {
        // Arrange
        var queryString = "?amount=250000&age=65&gender=Male&calculationMethod=EenLeven";

        // Act
        var response = await _client.GetAsync($"/api/v1/usufruct-calculations{queryString}");
        var result = await response.Content.ReadFromJsonAsync<UsufructResponse>();

        // Assert
        result.Should().NotBeNull();
        result!.CalculatedValue.Should().BeGreaterOrEqualTo(0);
    }

    [Fact]
    public async Task GetUsufructCalculation_WithoutApiKey_ReturnsUnauthorized()
    {
        // Arrange
        var clientWithoutKey = _factory.CreateClient();
        var queryString = "?amount=250000&age=65&gender=Male";

        // Act
        var response = await clientWithoutKey.GetAsync($"/api/v1/usufruct-calculations{queryString}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetUsufructCalculation_WithInvalidApiKey_ReturnsUnauthorized()
    {
        // Arrange
        var clientWithInvalidKey = _factory.CreateClient();
        clientWithInvalidKey.DefaultRequestHeaders.Add("X-API-Key", "invalid-key");
        var queryString = "?amount=250000&age=65&gender=Male";

        // Act
        var response = await clientWithInvalidKey.GetAsync($"/api/v1/usufruct-calculations{queryString}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetUsufructCalculation_WithMissingAmount_UsesDefaultZero()
    {
        // Arrange - Missing amount will default to 0
        var queryString = "?age=65&gender=Male";

        // Act
        var response = await _client.GetAsync($"/api/v1/usufruct-calculations{queryString}");

        // Assert - Should fail validation due to Amount range requirement
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("?amount=0&age=65&gender=Male")] // Amount too low
    [InlineData("?amount=250000&age=150&gender=Male")] // Age too high
    [InlineData("?amount=250000&age=-1&gender=Male")] // Age negative
    public async Task GetUsufructCalculation_WithInvalidParameterValues_ReturnsBadRequest(string queryString)
    {
        // Act
        var response = await _client.GetAsync($"/api/v1/usufruct-calculations{queryString}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(100000, 30, Gender.Female)]
    [InlineData(500000, 70, Gender.Male)]
    [InlineData(1, 0, Gender.Female)]
    [InlineData(int.MaxValue, 120, Gender.Male)]
    public async Task GetUsufructCalculation_WithVariousValidInputs_ReturnsOk(int amount, int age, Gender gender)
    {
        // Arrange
        var queryString = $"?amount={amount}&age={age}&gender={gender}";

        // Act
        var response = await _client.GetAsync($"/api/v1/usufruct-calculations{queryString}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
