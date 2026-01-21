namespace OutletRentalCars.Application.DTOs;

public class ReservationDto
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public int PickupLocationId { get; set; }
    public int ReturnLocationId { get; set; }
    public DateTime PickupDateTime { get; set; }
    public DateTime ReturnDateTime { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public bool IsActive { get; set; }
}