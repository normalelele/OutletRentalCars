using MediatR;
using OutletRentalCars.Application.DTOs;
using OutletRentalCars.Domain.Interfaces;
using OutletRentalCars.Domain.ValueObjects;

namespace OutletRentalCars.Application.Queries;

public class SearchVehiclesQueryHandler : IRequestHandler<SearchVehiclesQuery, IEnumerable<VehicleDto>>
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IMarketRepository _marketRepository;

    public SearchVehiclesQueryHandler(
        IVehicleRepository vehicleRepository,
        IReservationRepository reservationRepository,
        ILocationRepository locationRepository,
        IMarketRepository marketRepository)
    {
        _vehicleRepository = vehicleRepository;
        _reservationRepository = reservationRepository;
        _locationRepository = locationRepository;
        _marketRepository = marketRepository;
    }

    public async Task<IEnumerable<VehicleDto>> Handle(SearchVehiclesQuery request, CancellationToken cancellationToken)
    {        
        if (request.PickupDateTime >= request.ReturnDateTime)
            throw new ArgumentException("Pickup date must be before return date");

        var pickupLocation = await _locationRepository.GetByIdAsync(request.PickupLocationId);
        if (pickupLocation == null)
            throw new ArgumentException("Pickup location not found");
        
        var vehicles = await _vehicleRepository.GetAvailableVehiclesAsync(
            request.PickupLocationId,
            pickupLocation.CountryCode,
            request.PickupDateTime,
            request.ReturnDateTime);

        var searchDateRange = new DateRange(request.PickupDateTime, request.ReturnDateTime);
        var availableVehicles = new List<VehicleDto>();

        foreach (var vehicle in vehicles)
        {            
            var activeReservations = await _reservationRepository.GetActiveReservationsByVehicleIdAsync(vehicle.Id);

            bool hasConflict = activeReservations.Any(reservation =>
            {
                var reservationDateRange = reservation.GetDateRange();
                return reservationDateRange.OverlapsWith(searchDateRange);
            });
            
            if (!hasConflict && vehicle.IsAvailable())
            {
                availableVehicles.Add(new VehicleDto
                {
                    Id = vehicle.Id,
                    Brand = vehicle.Brand,
                    Model = vehicle.Model,
                    Year = vehicle.Year,
                    LicensePlate = vehicle.LicensePlate,
                    Status = vehicle.Status.ToString(),
                    MarketCode = vehicle.MarketCode,
                    Location = new LocationDto
                    {
                        Id = vehicle.Location.Id,
                        Name = vehicle.Location.Name,
                        City = vehicle.Location.City,
                        CountryCode = vehicle.Location.CountryCode
                    }
                });
            }
        }

        return availableVehicles;
    }
}