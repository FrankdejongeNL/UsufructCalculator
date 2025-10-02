using UsufructCalculator.Api.Models.Enums;
using UsufructCalculator.Api.Services.CalculationStrategies.Models;

namespace UsufructCalculator.Api.Services.CalculationStrategies;

/// <summary>
/// Interface for usufruct calculation strategies.
/// </summary>
public interface ICalculationStrategy
{
    /// <summary>
    /// Gets the calculation method this strategy implements.
    /// </summary>
    CalculationMethod Method { get; }

    /// <summary>
    /// Calculates the usufruct value.
    /// </summary>
    /// <param name="amount">The property value in euros.</param>
    /// <param name="age">The age of the usufructuary.</param>
    /// <param name="gender">The gender of the usufructuary.</param>
    /// <returns>The calculated usufruct value.</returns>
    UsufructResult Calculate(int amount, int age, Gender gender);
}
