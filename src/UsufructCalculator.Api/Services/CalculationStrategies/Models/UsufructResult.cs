namespace UsufructCalculator.Api.Services.CalculationStrategies.Models;

/// <summary>
/// Represents the internal calculation result for usufruct calculations.
/// </summary>
public record UsufructResult
{
    /// <summary>
    /// Gets the factor used during the calculation of the usufruct value.
    /// </summary>
    public decimal Factor { get; init; }

    /// <summary>
    /// Gets the calculated usufruct value.
    /// </summary>
    public decimal CalculatedValue { get; init; }
}
