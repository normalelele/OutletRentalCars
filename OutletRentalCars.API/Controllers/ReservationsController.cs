using MediatR;
using Microsoft.AspNetCore.Mvc;
using OutletRentalCars.Application.Commands;

namespace OutletRentalCars.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReservationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create a new vehicle reservation
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateReservation([FromBody] CreateReservationCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);

            return CreatedAtAction(
                nameof(CreateReservation),
                new { id = result.Id },
                new
                {
                    success = true,
                    message = "Reserva creada exitosamente",
                    data = result
                });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "Se produjo un error al crear la reserva.", details = ex.Message });
        }
    }
}