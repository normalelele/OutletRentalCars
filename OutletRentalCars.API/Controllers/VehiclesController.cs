using MediatR;
using Microsoft.AspNetCore.Mvc;
using OutletRentalCars.Application.Queries;

namespace OutletRentalCars.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly IMediator _mediator;

    public VehiclesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Search available vehicles based on pickup/return locations and dates
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchVehicles(
        [FromQuery] int pickupLocationId,
        [FromQuery] int returnLocationId,
        [FromQuery] DateTime pickupDateTime,
        [FromQuery] DateTime returnDateTime)
    {
        try
        {
            var query = new SearchVehiclesQuery(
                pickupLocationId,
                returnLocationId,
                pickupDateTime,
                returnDateTime
            );

            var result = await _mediator.Send(query);

            return Ok(new
            {
                success = true,
                data = result,
                count = result.Count()
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "Se produjo un error al buscar vehículos.", details = ex.Message });
        }
    }
}