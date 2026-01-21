using MediatR;

namespace OutletRentalCars.Domain.Events;

public class VehicleReservedEvent : INotification
{
    public int VehicleId { get; }
    public DateTime PickupDateTime { get; }
    public DateTime ReturnDateTime { get; }
    public DateTime OccurredOn { get; }

    public VehicleReservedEvent(int vehicleId, DateTime pickupDateTime, DateTime returnDateTime)
    {
        VehicleId = vehicleId;
        PickupDateTime = pickupDateTime;
        ReturnDateTime = returnDateTime;
        OccurredOn = DateTime.UtcNow;
    }
}