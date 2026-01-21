using OutletRentalCars.Domain.Entities;

namespace OutletRentalCars.Domain.Interfaces;

public interface IReservationRepository
{
    Task<IEnumerable<Reservation>> GetActiveReservationsByVehicleIdAsync(int vehicleId);
    Task<Reservation?> GetByIdAsync(int id);
    Task AddAsync(Reservation reservation);
    Task<IEnumerable<Reservation>> GetAllAsync();
}