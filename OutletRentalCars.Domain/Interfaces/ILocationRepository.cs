using OutletRentalCars.Domain.Entities;

namespace OutletRentalCars.Domain.Interfaces;

public interface ILocationRepository
{
    Task<Location?> GetByIdAsync(int id);
    Task<IEnumerable<Location>> GetAllAsync();
    Task AddAsync(Location location);
}