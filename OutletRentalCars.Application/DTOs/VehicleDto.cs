namespace OutletRentalCars.Application.DTOs;

public class VehicleDto
{
    public int Id { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string LicensePlate { get; set; }
    public string Status { get; set; }
    public LocationDto Location { get; set; }
    public string MarketCode { get; set; }
}