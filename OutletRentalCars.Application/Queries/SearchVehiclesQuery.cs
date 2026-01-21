using MediatR;
using OutletRentalCars.Application.DTOs;

namespace OutletRentalCars.Application.Queries;

public class SearchVehiclesQuery : IRequest<IEnumerable<VehicleDto>>
{
    public int PickupLocationId { get; set; }
    public int ReturnLocationId { get; set; }
    public DateTime PickupDateTime { get; set; }
    public DateTime ReturnDateTime { get; set; }

    public SearchVehiclesQuery(int pickupLocationId, int returnLocationId,
                               DateTime pickupDateTime, DateTime returnDateTime)
    {
        PickupLocationId = pickupLocationId;
        ReturnLocationId = returnLocationId;
        PickupDateTime = pickupDateTime;
        ReturnDateTime = returnDateTime;
    }
}