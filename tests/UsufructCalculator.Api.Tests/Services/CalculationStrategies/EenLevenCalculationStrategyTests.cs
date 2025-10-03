using FluentAssertions;
using UsufructCalculator.Api.Models.Enums;
using UsufructCalculator.Api.Services.CalculationStrategies;

namespace UsufructCalculator.Api.Tests.Services.CalculationStrategies;

/// <summary>
/// Unit tests for the EenLevenCalculationStrategy.
/// </summary>
public class EenLevenCalculationStrategyTests
{
    private readonly EenLevenCalculationStrategy _strategy;

    public EenLevenCalculationStrategyTests()
    {
        _strategy = new EenLevenCalculationStrategy();
    }

    [Fact]
    public void Method_ReturnsEenLeven()
    {
        // Act
        var result = _strategy.Method;

        // Assert
        result.Should().Be(CalculationMethod.EenLeven);
    }

    [Theory]
    [InlineData(100000, 20, Gender.Male, 22, 88000)]
    [InlineData(250000, 65, Gender.Male, 9, 90000)]
    [InlineData(500000, 45, Gender.Male, 16, 320000)]
    public void Calculate_WithMaleAge_ReturnsCorrectCalculation(
        int amount,
        int age,
        Gender gender,
        int expectedFactor,
        decimal expectedValue)
    {
        // Act
        var result = _strategy.Calculate(amount, age, gender);

        // Assert
        result.Factor.Should().Be(expectedFactor);
        result.CalculatedValue.Should().Be(expectedValue);
    }

    [Theory]
    [InlineData(100000, 25, Gender.Female, 22, 88000)] // 25 - 5 = 20, factor 22
    [InlineData(250000, 70, Gender.Female, 9, 90000)]  // 70 - 5 = 65, factor 9
    [InlineData(500000, 50, Gender.Female, 16, 320000)] // 50 - 5 = 45, factor 16
    public void Calculate_WithFemaleAge_AdjustsAgeAndReturnsCorrectCalculation(
        int amount,
        int age,
        Gender gender,
        int expectedFactor,
        decimal expectedValue)
    {
        // Act
        var result = _strategy.Calculate(amount, age, gender);

        // Assert
        result.Factor.Should().Be(expectedFactor);
        result.CalculatedValue.Should().Be(expectedValue);
    }

    [Fact]
    public void Calculate_YoungMale_UsesHighestFactor()
    {
        // Arrange
        int amount = 100000;
        int age = 20;
        Gender gender = Gender.Male;

        // Act
        var result = _strategy.Calculate(amount, age, gender);

        // Assert
        result.Factor.Should().Be(22); // Highest factor
        result.CalculatedValue.Should().Be(88000); // 100000 * 0.04 * 22
    }

    [Fact]
    public void Calculate_OldMale_UsesLowestFactor()
    {
        // Arrange
        int amount = 100000;
        int age = 100;
        Gender gender = Gender.Male;

        // Act
        var result = _strategy.Calculate(amount, age, gender);

        // Assert
        result.Factor.Should().Be(1); // Lowest factor
        result.CalculatedValue.Should().Be(4000); // 100000 * 0.04 * 1
    }

    [Fact]
    public void Calculate_FemaleAge30_AdjustsTo25()
    {
        // Arrange
        int amount = 200000;
        int age = 30; // Adjusted to 25 for female
        Gender gender = Gender.Female;

        // Act
        var result = _strategy.Calculate(amount, age, gender);

        // Assert - Age 25 should use factor 21
        result.Factor.Should().Be(21);
        result.CalculatedValue.Should().Be(168000); // 200000 * 0.04 * 21
    }

    [Theory]
    [InlineData(1, 20, Gender.Male, 22, 1)] // Minimum amount
    [InlineData(1000000, 20, Gender.Male, 22, 880000)] // Large amount
    public void Calculate_WithVariousAmounts_ReturnsCorrectCalculation(
        int amount,
        int age,
        Gender gender,
        int expectedFactor,
        decimal expectedValue)
    {
        // Act
        var result = _strategy.Calculate(amount, age, gender);

        // Assert
        result.Factor.Should().Be(expectedFactor);
        result.CalculatedValue.Should().BeApproximately(expectedValue, 0.01m);
    }

    [Theory]
    [InlineData(100000, 0, Gender.Male, 22, 88000)]
    [InlineData(100000, 120, Gender.Male, 1, 4000)]
    public void Calculate_WithBoundaryAges_ReturnsCorrectCalculation(
        int amount,
        int age,
        Gender gender,
        int expectedFactor,
        decimal expectedValue)
    {
        // Act
        var result = _strategy.Calculate(amount, age, gender);

        // Assert
        result.Factor.Should().Be(expectedFactor);
        result.CalculatedValue.Should().Be(expectedValue);
    }

    [Fact]
    public void Calculate_Formula_UsesCorrectCalculation()
    {
        // Arrange - Test that formula is: amount * 0.04 * factor
        int amount = 150000;
        int age = 35; // Factor 19
        Gender gender = Gender.Male;
        const decimal CalculationValue = 0.04M;

        // Act
        var result = _strategy.Calculate(amount, age, gender);

        // Assert
        var expectedValue = amount * CalculationValue * 19;
        result.CalculatedValue.Should().Be(expectedValue);
    }

    [Theory]
    [InlineData(24, Gender.Male, 22)]
    [InlineData(25, Gender.Male, 21)]
    [InlineData(29, Gender.Male, 21)]
    [InlineData(30, Gender.Male, 20)]
    [InlineData(34, Gender.Male, 20)]
    [InlineData(35, Gender.Male, 19)]
    public void Calculate_AgeBoundaries_UsesCorrectFactor(int age, Gender gender, int expectedFactor)
    {
        // Arrange
        int amount = 100000;

        // Act
        var result = _strategy.Calculate(amount, age, gender);

        // Assert
        result.Factor.Should().Be(expectedFactor);
    }

    [Fact]
    public void Calculate_FemaleYoungAge_CanResultInNegativeAdjustedAge()
    {
        // Arrange - Young female where adjusted age becomes negative
        int amount = 100000;
        int age = 3; // Adjusted to -2, but should still use highest factor
        Gender gender = Gender.Female;

        // Act
        var result = _strategy.Calculate(amount, age, gender);

        // Assert - Should handle negative adjusted age gracefully
        // This might throw or use highest factor - depends on requirements
        result.Factor.Should().BeGreaterThan(0);
        result.CalculatedValue.Should().BeGreaterThan(0);
    }

    [Fact]
    public void Calculate_AssignmentValue_roundedUp()
    {
        // Arrange
        int amount = 1234;
        int age = 26;
        Gender gender = Gender.Male;
        decimal expectedFactor = 21.00M;

        //Act
        var result = _strategy.Calculate(amount, age, gender);

        //assert
        result.Factor.Should().Be(expectedFactor);
        result.CalculatedValue.Should().Be(1037M);
    }
}
