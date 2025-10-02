using FluentAssertions;
using UsufructCalculator.Api.Services.CalculationStrategies.Models;

namespace UsufructCalculator.Api.Tests.Services.CalculationStrategies.Models;

/// <summary>
/// Unit tests for the EenLevenAgeFactorTable.
/// </summary>
public class EenLevenAgeFactorTableTests
{
    [Theory]
    [InlineData(0, 22)]
    [InlineData(10, 22)]
    [InlineData(24, 22)]
    public void GetFactor_AgeUnder25_Returns22(int age, int expectedFactor)
    {
        // Act
        var result = EenLevenAgeFactorTable.GetFactor(age);

        // Assert
        result.Should().Be(expectedFactor);
    }

    [Theory]
    [InlineData(25, 21)]
    [InlineData(29, 21)]
    public void GetFactor_AgeBetween25And29_Returns21(int age, int expectedFactor)
    {
        // Act
        var result = EenLevenAgeFactorTable.GetFactor(age);

        // Assert
        result.Should().Be(expectedFactor);
    }

    [Theory]
    [InlineData(30, 20)]
    [InlineData(34, 20)]
    public void GetFactor_AgeBetween30And34_Returns20(int age, int expectedFactor)
    {
        // Act
        var result = EenLevenAgeFactorTable.GetFactor(age);

        // Assert
        result.Should().Be(expectedFactor);
    }

    [Theory]
    [InlineData(35, 19)]
    [InlineData(39, 19)]
    [InlineData(40, 18)]
    [InlineData(44, 18)]
    [InlineData(45, 16)]
    [InlineData(49, 16)]
    [InlineData(50, 15)]
    [InlineData(54, 15)]
    [InlineData(55, 13)]
    [InlineData(59, 13)]
    [InlineData(60, 11)]
    [InlineData(64, 11)]
    [InlineData(65, 9)]
    [InlineData(69, 9)]
    [InlineData(70, 8)]
    [InlineData(74, 8)]
    [InlineData(75, 6)]
    [InlineData(79, 6)]
    [InlineData(80, 4)]
    [InlineData(84, 4)]
    [InlineData(85, 3)]
    [InlineData(89, 3)]
    [InlineData(90, 2)]
    [InlineData(94, 2)]
    public void GetFactor_VariousAges_ReturnsCorrectFactor(int age, int expectedFactor)
    {
        // Act
        var result = EenLevenAgeFactorTable.GetFactor(age);

        // Assert
        result.Should().Be(expectedFactor);
    }

    [Theory]
    [InlineData(95, 1)]
    [InlineData(100, 1)]
    [InlineData(119, 1)]
    [InlineData(120, 1)]
    public void GetFactor_AgeAbove94_ReturnsLowestFactor(int age, int expectedFactor)
    {
        // Act
        var result = EenLevenAgeFactorTable.GetFactor(age);

        // Assert
        result.Should().Be(expectedFactor);
    }

    [Fact]
    public void GetFactor_NegativeAge_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var negativeAge = -1;

        // Act
        Action act = () => EenLevenAgeFactorTable.GetFactor(negativeAge);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Age cannot be negative*");
    }

    [Fact]
    public void GetFactor_BoundaryAge24_Returns22()
    {
        // Arrange & Act
        var result = EenLevenAgeFactorTable.GetFactor(24);

        // Assert
        result.Should().Be(22);
    }

    [Fact]
    public void GetFactor_BoundaryAge25_Returns21()
    {
        // Arrange & Act
        var result = EenLevenAgeFactorTable.GetFactor(25);

        // Assert
        result.Should().Be(21);
    }
}
