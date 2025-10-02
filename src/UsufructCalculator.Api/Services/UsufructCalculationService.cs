using UsufructCalculator.Api.Models;
using UsufructCalculator.Api.Services.CalculationStrategies;
using UsufructCalculator.Api.Services.CalculationStrategies.Models;

namespace UsufructCalculator.Api.Services;

/// <summary>
/// Service implementation for calculating usufruct values.
/// </summary>
public class UsufructCalculationService : IUsufructCalculationService
{
    private readonly IEnumerable<ICalculationStrategy> _calculationStrategies;

    /// <summary>
    /// Initializes a new instance of the <see cref="UsufructCalculationService"/> class.
    /// </summary>
    /// <param name="calculationStrategies">The collection of available calculation strategies.</param>
    public UsufructCalculationService(IEnumerable<ICalculationStrategy> calculationStrategies)
    {
        _calculationStrategies = calculationStrategies;
    }

    /// <summary>
    /// Calculates the usufruct value based on the provided request parameters.
    /// </summary>
    /// <param name="request">The calculation request parameters.</param>
    /// <returns>The calculated usufruct response.</returns>
    /// <exception cref="NotImplementedException">Thrown when the requested calculation method is not implemented.</exception>
    public UsufructResponse Calculate(UsufructRequest request)
    {
        var strategy = _calculationStrategies.FirstOrDefault(s => s.Method == request.CalculationMethod);

        if (strategy == null)
        {
            throw new NotImplementedException($"Calculation method {request.CalculationMethod} is not implemented");
        }

        UsufructResult calculatedValue = strategy.Calculate(request.Amount, request.Age, request.Gender);

        return new UsufructResponse
        {
            OriginalValue = request.Amount,
            CalculatedValue = calculatedValue.CalculatedValue,
            Factor = calculatedValue.Factor,
        };
    }
}
