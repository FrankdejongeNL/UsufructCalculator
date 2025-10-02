namespace UsufructCalculator.Api.Services.CalculationStrategies.Models;

/// <summary>
/// Provides age-based factor lookup for Een Leven usufruct calculations.
/// </summary>
public static class EenLevenAgeFactorTable
{
    /// <summary>
    /// Table mapping maximum age to corresponding calculation factor used for Een Leven calculations.
    /// Each entry represents: (MaxAge, Factor) where Factor applies if age is less than MaxAge.
    /// </summary>
    private static readonly (int MaxAge, int Factor)[] _table =
    {
        (25, 22),
        (30, 21),
        (35, 20),
        (40, 19),
        (45, 18),
        (50, 16),
        (55, 15),
        (60, 13),
        (65, 11),
        (70, 9),
        (75, 8),
        (80, 6),
        (85, 4),
        (90, 3),
        (95, 2),
        (120, 1),
    };

    /// <summary>
    /// Gets the calculation factor for a given age.
    /// </summary>
    /// <param name="age">The age of the usufructuary.</param>
    /// <returns>The factor to use in the calculation.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when age is negative.</exception>
    public static int GetFactor(int age)
    {
        if (age < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(age), "Age cannot be negative.");
        }

        foreach (var (maxAge, factor) in _table)
        {
            if (age < maxAge)
            {
                return factor;
            }
        }

        // For ages beyond the last entry in the table
        return _table[^1].Factor;
    }
}
