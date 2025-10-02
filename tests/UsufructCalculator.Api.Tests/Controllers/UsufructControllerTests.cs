using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UsufructCalculator.Api.Controllers;
using UsufructCalculator.Api.Models;
using UsufructCalculator.Api.Models.Enums;
using UsufructCalculator.Api.Services;

namespace UsufructCalculator.Api.Tests.Controllers;

/// <summary>
/// Unit tests for the UsufructController
/// </summary>
public class UsufructControllerTests
{
    private readonly Mock<ILogger<UsufructController>> _loggerMock;
    private readonly Mock<IUsufructCalculationService> _calculationServiceMock;
    private readonly UsufructController _controller;
    private readonly UsufructRequest _defaultRequest;

    public UsufructControllerTests()
    {
        _loggerMock = new Mock<ILogger<UsufructController>>();
        _calculationServiceMock = new Mock<IUsufructCalculationService>();
        _controller = new UsufructController(_loggerMock.Object, _calculationServiceMock.Object);
        _defaultRequest = new UsufructRequest
        {
            Amount = 250000,
            Age = 65,
            Gender = Gender.Male,
            CalculationMethod = CalculationMethod.EenLeven
        };
    }

    [Fact]
    public void Get_WithValidRequest_ReturnsOkResult()
    {
        // Arrange
        var request = _defaultRequest;

        _calculationServiceMock
            .Setup(s => s.Calculate(It.IsAny<UsufructRequest>()))
            .Returns(new UsufructResponse { CalculatedValue = 100000 });

        // Act
        var result = _controller.Get(request);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public void Get_WithValidRequest_ReturnsUsufructResponse()
    {
        // Arrange
        var request = _defaultRequest;

        _calculationServiceMock
            .Setup(s => s.Calculate(It.IsAny<UsufructRequest>()))
            .Returns(new UsufructResponse { CalculatedValue = 100000 });

        // Act
        var result = _controller.Get(request);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeOfType<UsufructResponse>();
    }

    [Fact]
    public void Get_WithValidRequest_LogsInformation()
    {
        // Arrange
        var request = _defaultRequest;

        _calculationServiceMock
            .Setup(s => s.Calculate(It.IsAny<UsufructRequest>()))
            .Returns(new UsufructResponse { CalculatedValue = 100000 });

        // Act
        _controller.Get(request);

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Usufruct calculation requested")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Theory]
    [InlineData(100000, 30, Gender.Female)]
    [InlineData(500000, 70, Gender.Male)]
    [InlineData(1, 0, Gender.Female)]
    public void Get_WithDifferentValidInputs_ReturnsOkResult(int amount, int age, Gender gender)
    {
        // Arrange
        var request = new UsufructRequest
        {
            Amount = amount,
            Age = age,
            Gender = gender,
            CalculationMethod = CalculationMethod.EenLeven
        };

        _calculationServiceMock
            .Setup(s => s.Calculate(It.IsAny<UsufructRequest>()))
            .Returns(new UsufructResponse { CalculatedValue = 100000 });

        // Act
        var result = _controller.Get(request);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
    }
}
