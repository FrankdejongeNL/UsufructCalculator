using UsufructCalculator.Api.Models.Enums;
using UsufructCalculator.Api.Services.CalculationStrategies.Models;

namespace UsufructCalculator.Api.Services.CalculationStrategies;

/// <summary>
/// Calculation strategy for the "Eén leven" (single life) method.
/// </summary>
public class EenLevenCalculationStrategy : ICalculationStrategy
{
    /// <summary>
    /// Gets the calculation method this strategy implements.
    /// </summary>
    public CalculationMethod Method => CalculationMethod.EenLeven;

    /// <summary>
    /// Calculates the usufruct value using the "Eén leven" method.
    /// </summary>
    /// <param name="amount">The property value in euros.</param>
    /// <param name="age">The age of the usufructuary.</param>
    /// <param name="gender">The gender of the usufructuary.</param>
    /// <returns>The calculated usufruct value.</returns>
    public UsufructResult Calculate(int amount, int age, Gender gender)
    {
        const decimal CalculationValue = 0.04M;
        var factor = DetermineFactor(age, gender);

        return new UsufructResult
        {
            Factor = factor,

            // For tax values we round the number up in favor of the user.
            CalculatedValue = Math.Ceiling(amount * CalculationValue * factor),
        };
    }

    private decimal DetermineFactor(int age, Gender gender)
    {
        const int FemaleMinimumAge = 4;
        var adjustedAge = age;

        // Adjust age for female gender (subtract 5 years)
        if (gender == Gender.Female && age > FemaleMinimumAge)
        {
            adjustedAge -= 5;
        }

        // Get factor from lookup table
        return EenLevenAgeFactorTable.GetFactor(adjustedAge);
    }
}
