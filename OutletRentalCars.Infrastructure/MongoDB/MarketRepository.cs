using MongoDB.Driver;
using OutletRentalCars.Domain.Entities;
using OutletRentalCars.Domain.Interfaces;

namespace OutletRentalCars.Infrastructure.MongoDB;

public class MarketRepository : IMarketRepository
{
    private readonly MongoDbContext _context;

    public MarketRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<Market?> GetByCodeAsync(string code)
    {
        return await _context.Markets
            .Find(m => m.Code == code)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Market>> GetAllActiveAsync()
    {
        return await _context.Markets
            .Find(m => m.IsActive)
            .ToListAsync();
    }

    public async Task AddAsync(Market market)
    {
        await _context.Markets.InsertOneAsync(market);
    }
}