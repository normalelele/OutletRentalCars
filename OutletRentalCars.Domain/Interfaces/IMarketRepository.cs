using OutletRentalCars.Domain.Entities;

namespace OutletRentalCars.Domain.Interfaces;

public interface IMarketRepository
{
    Task<Market?> GetByCodeAsync(string code);
    Task<IEnumerable<Market>> GetAllActiveAsync();
    Task AddAsync(Market market);
}