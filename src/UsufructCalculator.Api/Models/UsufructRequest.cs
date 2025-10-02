using System.ComponentModel.DataAnnotations;
using UsufructCalculator.Api.Models.Enums;

namespace UsufructCalculator.Api.Models;

/// <summary>
/// Request model for usufruct calculation.
/// </summary>
public record UsufructRequest
{
    /// <summary>
    /// Gets the property value in full euros (no decimals).
    /// </summary>
    [Required(ErrorMessage = "Amount is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Amount must be at least 1 euro")]
    public int Amount { get; init; }

    /// <summary>
    /// Gets the age of the usufructuary.
    /// </summary>
    [Required(ErrorMessage = "Age is required")]
    [Range(0, 120, ErrorMessage = "Age must be between 0 and 120")]
    public int Age { get; init; }

    /// <summary>
    /// Gets the gender of the usufructuary.
    /// </summary>
    [Required(ErrorMessage = "Gender is required")]
    public Gender Gender { get; init; }

    /// <summary>
    /// Gets the calculation method to use (default: EenLeven).
    /// </summary>
    public CalculationMethod CalculationMethod { get; init; } = CalculationMethod.EenLeven;
}
