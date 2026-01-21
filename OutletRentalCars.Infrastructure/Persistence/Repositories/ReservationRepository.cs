using Microsoft.EntityFrameworkCore;
using OutletRentalCars.Domain.Entities;
using OutletRentalCars.Domain.Interfaces;

namespace OutletRentalCars.Infrastructure.Persistence.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly ApplicationDbContext _context;

    public ReservationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Reservation>> GetActiveReservationsByVehicleIdAsync(int vehicleId)
    {
        return await _context.Reservations
            .Where(r => r.VehicleId == vehicleId && r.IsActive)
            .ToListAsync();
    }

    public async Task<Reservation?> GetByIdAsync(int id)
    {
        return await _context.Reservations
            .Include(r => r.Vehicle)
            .Include(r => r.PickupLocation)
            .Include(r => r.ReturnLocation)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task AddAsync(Reservation reservation)
    {
        await _context.Reservations.AddAsync(reservation);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Reservation>> GetAllAsync()
    {
        return await _context.Reservations
            .Include(r => r.Vehicle)
            .Include(r => r.PickupLocation)
            .Include(r => r.ReturnLocation)
            .ToListAsync();
    }
}