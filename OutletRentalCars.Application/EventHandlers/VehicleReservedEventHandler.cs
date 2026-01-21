using MediatR;
using OutletRentalCars.Domain.Events;

namespace OutletRentalCars.Application.EventHandlers;

public class VehicleReservedEventHandler : INotificationHandler<VehicleReservedEvent>
{
    public Task Handle(VehicleReservedEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[EVENT] Vehículo {notification.VehicleId} reservado de {notification.PickupDateTime} a {notification.ReturnDateTime}");

        return Task.CompletedTask;
    }
}