using Microsoft.EntityFrameworkCore;
using OutletRentalCars.Domain.Entities;
using OutletRentalCars.Domain.Enums;
using OutletRentalCars.Domain.Interfaces;

namespace OutletRentalCars.Infrastructure.Persistence.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly ApplicationDbContext _context;

    public VehicleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Vehicle?> GetByIdAsync(int id)
    {
        return await _context.Vehicles
            .Include(v => v.Location)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<IEnumerable<Vehicle>> GetAvailableVehiclesAsync(
        int pickupLocationId,
        string marketCode,
        DateTime pickupDate,
        DateTime returnDate)
    {
        return await _context.Vehicles
            .Include(v => v.Location)
            .Where(v => v.LocationId == pickupLocationId
                     && v.MarketCode == marketCode
                     && v.Status == VehicleStatus.Available)
            .ToListAsync();
    }

    public async Task<IEnumerable<Vehicle>> GetAllAsync()
    {
        return await _context.Vehicles
            .Include(v => v.Location)
            .ToListAsync();
    }

    public async Task AddAsync(Vehicle vehicle)
    {
        await _context.Vehicles.AddAsync(vehicle);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Vehicle vehicle)
    {
        _context.Vehicles.Update(vehicle);
        await _context.SaveChangesAsync();
    }
}