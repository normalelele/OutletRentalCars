using MediatR;
using OutletRentalCars.Application.DTOs;
using OutletRentalCars.Domain.Entities;
using OutletRentalCars.Domain.Interfaces;
using OutletRentalCars.Domain.ValueObjects;

namespace OutletRentalCars.Application.Commands;

public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, ReservationDto>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IMediator _mediator;

    public CreateReservationCommandHandler(
        IReservationRepository reservationRepository,
        IVehicleRepository vehicleRepository,
        IMediator mediator)
    {
        _reservationRepository = reservationRepository;
        _vehicleRepository = vehicleRepository;
        _mediator = mediator;
    }

    public async Task<ReservationDto> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId);
        if (vehicle == null)
            throw new ArgumentException("Vehicle not found");

        if (!vehicle.IsAvailable())
            throw new InvalidOperationException("Vehicle is not available");

        var requestDateRange = new DateRange(request.PickupDateTime, request.ReturnDateTime);
        var activeReservations = await _reservationRepository.GetActiveReservationsByVehicleIdAsync(request.VehicleId);

        bool hasConflict = activeReservations.Any(r => r.GetDateRange().OverlapsWith(requestDateRange));
        if (hasConflict)
            throw new InvalidOperationException("Vehicle already has a reservation for the selected dates");
      
        var reservation = new Reservation(
            request.VehicleId,
            request.PickupLocationId,
            request.ReturnLocationId,
            request.PickupDateTime,
            request.ReturnDateTime,
            request.CustomerName,
            request.CustomerEmail
        );

        await _reservationRepository.AddAsync(reservation);
        
        vehicle.Reserve();
        await _vehicleRepository.UpdateAsync(vehicle);
        
        foreach (var domainEvent in reservation.DomainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }
        reservation.ClearDomainEvents();

        return new ReservationDto
        {
            Id = reservation.Id,
            VehicleId = reservation.VehicleId,
            PickupLocationId = reservation.PickupLocationId,
            ReturnLocationId = reservation.ReturnLocationId,
            PickupDateTime = reservation.PickupDateTime,
            ReturnDateTime = reservation.ReturnDateTime,
            CustomerName = reservation.CustomerName,
            CustomerEmail = reservation.CustomerEmail,
            IsActive = reservation.IsActive
        };
    }
}