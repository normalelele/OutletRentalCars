using OutletRentalCars.Domain.Entities;

namespace OutletRentalCars.Domain.Interfaces;

public interface IVehicleRepository
{
    Task<Vehicle?> GetByIdAsync(int id);
    Task<IEnumerable<Vehicle>> GetAvailableVehiclesAsync(
        int pickupLocationId,
        string marketCode,
        DateTime pickupDate,
        DateTime returnDate);
    Task<IEnumerable<Vehicle>> GetAllAsync();
    Task AddAsync(Vehicle vehicle);
    Task UpdateAsync(Vehicle vehicle);
}