using OutletRentalCars.Domain.Events;
using OutletRentalCars.Domain.ValueObjects;

namespace OutletRentalCars.Domain.Entities;

public class Reservation : BaseEntity
{
    public int Id { get; private set; }
    public int VehicleId { get; private set; }
    public int PickupLocationId { get; private set; }
    public int ReturnLocationId { get; private set; }
    public DateTime PickupDateTime { get; private set; }
    public DateTime ReturnDateTime { get; private set; }
    public string CustomerName { get; private set; }
    public string CustomerEmail { get; private set; }
    public bool IsActive { get; private set; }
    
    public Vehicle Vehicle { get; private set; }
    public Location PickupLocation { get; private set; }
    public Location ReturnLocation { get; private set; }

    private Reservation() { }

    public Reservation(int vehicleId, int pickupLocationId, int returnLocationId,
                       DateTime pickupDateTime, DateTime returnDateTime,
                       string customerName, string customerEmail)
    {
        VehicleId = vehicleId;
        PickupLocationId = pickupLocationId;
        ReturnLocationId = returnLocationId;
        PickupDateTime = pickupDateTime;
        ReturnDateTime = returnDateTime;
        CustomerName = customerName;
        CustomerEmail = customerEmail;
        IsActive = true;
        
        AddDomainEvent(new VehicleReservedEvent(vehicleId, pickupDateTime, returnDateTime));
    }
    
    public DateRange GetDateRange()
    {
        return new DateRange(PickupDateTime, ReturnDateTime);
    }
    
    public bool IsActiveReservation()
    {
        return IsActive;
    }

    public void Cancel()
    {
        IsActive = false;
    }
}