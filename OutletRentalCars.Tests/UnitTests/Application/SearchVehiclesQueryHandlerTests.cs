using FluentAssertions;
using Moq;
using OutletRentalCars.Application.Queries;
using OutletRentalCars.Domain.Entities;
using OutletRentalCars.Domain.Interfaces;

namespace OutletRentalCars.Tests.UnitTests.Application;

public class SearchVehiclesQueryHandlerTests
{
    private readonly Mock<IVehicleRepository> _vehicleRepositoryMock;
    private readonly Mock<IReservationRepository> _reservationRepositoryMock;
    private readonly Mock<ILocationRepository> _locationRepositoryMock;
    private readonly Mock<IMarketRepository> _marketRepositoryMock;
    private readonly SearchVehiclesQueryHandler _handler;

    public SearchVehiclesQueryHandlerTests()
    {
        _vehicleRepositoryMock = new Mock<IVehicleRepository>();
        _reservationRepositoryMock = new Mock<IReservationRepository>();
        _locationRepositoryMock = new Mock<ILocationRepository>();
        _marketRepositoryMock = new Mock<IMarketRepository>();

        _handler = new SearchVehiclesQueryHandler(
            _vehicleRepositoryMock.Object,
            _reservationRepositoryMock.Object,
            _locationRepositoryMock.Object,
            _marketRepositoryMock.Object
        );
    }

    [Fact]
    public async Task Handle_WhenPickupDateIsAfterReturnDate_ShouldThrowArgumentException()
    {        
        var query = new SearchVehiclesQuery(
            pickupLocationId: 1,
            returnLocationId: 2,
            pickupDateTime: new DateTime(2026, 2, 10),
            returnDateTime: new DateTime(2026, 2, 5)
        );
        
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);
        
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("La fecha de recogida debe ser anterior a la fecha de devolución.");
    }

    [Fact]
    public async Task Handle_WhenLocationNotFound_ShouldThrowArgumentException()
    {        
        var query = new SearchVehiclesQuery(1, 2,
            new DateTime(2026, 2, 1), new DateTime(2026, 2, 5));

        _locationRepositoryMock.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((Location)null);
        
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);
        
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Lugar de recogida no encontrado");
    }

    [Fact]
    public async Task Handle_WhenNoConflictingReservations_ShouldReturnAvailableVehicles()
    {        
        var pickupLocation = new Location(1, "Bogotá Airport", "Bogotá", "CO");
        var vehicle = new Vehicle(1, "Toyota", "Corolla", 2023, "ABC123", 1, "CO");

        var query = new SearchVehiclesQuery(1, 2,
            new DateTime(2026, 2, 1), new DateTime(2026, 2, 5));

        _locationRepositoryMock.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(pickupLocation);

        _vehicleRepositoryMock.Setup(x => x.GetAvailableVehiclesAsync(
            1, "CO", It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new List<Vehicle> { vehicle });

        _reservationRepositoryMock.Setup(x => x.GetActiveReservationsByVehicleIdAsync(1))
            .ReturnsAsync(new List<Reservation>());
        
        var result = await _handler.Handle(query, CancellationToken.None);
        
        result.Should().HaveCount(1);
        result.First().Brand.Should().Be("Toyota");
        result.First().Model.Should().Be("Corolla");
    }

    [Fact]
    public async Task Handle_WhenVehicleHasConflictingReservation_ShouldNotReturnVehicle()
    {        
        var pickupLocation = new Location(1, "Bogotá Airport", "Bogotá", "CO");
        var vehicle = new Vehicle(1, "Toyota", "Corolla", 2023, "ABC123", 1, "CO");

        var conflictingReservation = new Reservation(
            vehicleId: 1,
            pickupLocationId: 1,
            returnLocationId: 2,
            pickupDateTime: new DateTime(2026, 2, 3),
            returnDateTime: new DateTime(2026, 2, 7),
            customerName: "Test User",
            customerEmail: "test@test.com"
        );

        var query = new SearchVehiclesQuery(1, 2,
            new DateTime(2026, 2, 1), new DateTime(2026, 2, 5));

        _locationRepositoryMock.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(pickupLocation);

        _vehicleRepositoryMock.Setup(x => x.GetAvailableVehiclesAsync(
            1, "CO", It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new List<Vehicle> { vehicle });

        _reservationRepositoryMock.Setup(x => x.GetActiveReservationsByVehicleIdAsync(1))
            .ReturnsAsync(new List<Reservation> { conflictingReservation });
        
        var result = await _handler.Handle(query, CancellationToken.None);
        
        result.Should().BeEmpty();
    }
}