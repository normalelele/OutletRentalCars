using OutletRentalCars.Domain.Enums;

namespace OutletRentalCars.Domain.Entities;

public class Vehicle : BaseEntity
{
    public int Id { get; private set; }
    public string Brand { get; private set; }
    public string Model { get; private set; }
    public int Year { get; private set; }
    public string LicensePlate { get; private set; }
    public VehicleStatus Status { get; private set; }
    public int LocationId { get; private set; }
    public string MarketCode { get; private set; }
    
    public Location Location { get; private set; }
    
    private Vehicle() { }

    public Vehicle(int id, string brand, string model, int year,
                   string licensePlate, int locationId, string marketCode)
    {
        Id = id;
        Brand = brand;
        Model = model;
        Year = year;
        LicensePlate = licensePlate;
        Status = VehicleStatus.Available;
        LocationId = locationId;
        MarketCode = marketCode;
    }
    
    public bool IsAvailable()
    {
        return Status == VehicleStatus.Available;
    }
    
    public bool IsEnabledForMarket(string marketCode)
    {
        return MarketCode.Equals(marketCode, StringComparison.OrdinalIgnoreCase);
    }
    
    public void Reserve()
    {
        if (!IsAvailable())
            throw new InvalidOperationException("Vehicle is not available for reservation");

        Status = VehicleStatus.Reserved;
    }

    public void MakeAvailable()
    {
        Status = VehicleStatus.Available;
    }
}