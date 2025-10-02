using Microsoft.AspNetCore.Mvc;
using UsufructCalculator.Api.Models;

namespace UsufructCalculator.Api.Controllers;

/// <summary>
/// Controller for usufruct calculations.
/// </summary>
[ApiController]
[Route("api/usufruct-calculations")]
[Produces("application/json")]
public class UsufructController : ControllerBase
{
    private readonly ILogger<UsufructController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UsufructController"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public UsufructController(ILogger<UsufructController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Calculates the usufruct value based on provided parameters.
    /// </summary>
    /// <param name="request">The calculation parameters.</param>
    /// <returns>The calculated usufruct value.</returns>
    /// <response code="200">Returns the calculated usufruct value.</response>
    /// <response code="400">If the request parameters are invalid.</response>
    /// <response code="500">If an error occurs during calculation.</response>
    [HttpGet]
    [ProducesResponseType(typeof(UsufructResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public ActionResult<UsufructResponse> Get([FromQuery] UsufructRequest request)
    {
        _logger.LogInformation(
            "Usufruct calculation requested - Amount: {Amount}, Age: {Age}, Gender: " +
            "{Gender}, Method: {Method}",
            request.Amount,
            request.Age,
            request.Gender,
            request.CalculationMethod);

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for usufruct calculation");
            return BadRequest(ModelState);
        }

        try
        {
            // TODO: Implement usufruct calculation logic
            var response = new UsufructResponse
            {
                CalculatedValue = 0,
            };

            _logger.LogInformation("Usufruct calculation completed successfully");
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during usufruct calculation");
            return Problem(
                title: "Calculation Error",
                detail: "An error occurred while processing the usufruct calculation",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
