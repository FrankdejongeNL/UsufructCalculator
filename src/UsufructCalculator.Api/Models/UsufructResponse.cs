namespace UsufructCalculator.Api.Models;

/// <summary>
/// Response model for usufruct calculation.
/// </summary>
public record UsufructResponse
{
    /// <summary>
    /// Gets the value used to calculate the usufruct value with.
    /// </summary>
    public decimal OriginalValue { get; init; }

    /// <summary>
    /// Gets The factor used during the calculation of the usufruct value.
    /// </summary>
    public decimal Factor { get; init; }

    /// <summary>
    /// Gets the calculated usufruct value.
    /// </summary>
    public decimal CalculatedValue { get; init; }
}
