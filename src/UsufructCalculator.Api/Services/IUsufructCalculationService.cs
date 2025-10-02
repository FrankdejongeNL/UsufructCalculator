using UsufructCalculator.Api.Models;

namespace UsufructCalculator.Api.Services;

/// <summary>
/// Service for calculating usufruct values.
/// </summary>
public interface IUsufructCalculationService
{
    /// <summary>
    /// Calculates the usufruct value based on the provided request parameters.
    /// </summary>
    /// <param name="request">The calculation request parameters.</param>
    /// <returns>The calculated usufruct response.</returns>
    UsufructResponse Calculate(UsufructRequest request);
}
