using MediatR;
using OutletRentalCars.Application.DTOs;

namespace OutletRentalCars.Application.Commands;

public class CreateReservationCommand : IRequest<ReservationDto>
{
    public int VehicleId { get; set; }
    public int PickupLocationId { get; set; }
    public int ReturnLocationId { get; set; }
    public DateTime PickupDateTime { get; set; }
    public DateTime ReturnDateTime { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
}