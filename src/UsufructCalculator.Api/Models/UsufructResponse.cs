namespace UsufructCalculator.Api.Models;

/// <summary>
/// Response model for usufruct calculation.
/// </summary>
public record UsufructResponse
{
    /// <summary>
    /// Gets the calculated usufruct value.
    /// </summary>
    public decimal CalculatedValue { get; init; }
}
