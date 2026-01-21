using FluentAssertions;
using OutletRentalCars.Domain.Entities;
using OutletRentalCars.Domain.Enums;

namespace OutletRentalCars.Tests.UnitTests.Domain;

public class VehicleTests
{
    [Fact]
    public void Constructor_ShouldCreateVehicleWithAvailableStatus()
    {       
        var vehicle = new Vehicle(1, "Toyota", "Corolla", 2023, "ABC123", 1, "CO");
        
        vehicle.Status.Should().Be(VehicleStatus.Available);
        vehicle.IsAvailable().Should().BeTrue();
    }

    [Fact]
    public void IsEnabledForMarket_WhenMarketMatches_ShouldReturnTrue()
    {        
        var vehicle = new Vehicle(1, "Toyota", "Corolla", 2023, "ABC123", 1, "CO");
        
        var result = vehicle.IsEnabledForMarket("CO");
        
        result.Should().BeTrue();
    }

    [Fact]
    public void IsEnabledForMarket_WhenMarketDoesNotMatch_ShouldReturnFalse()
    {       
        var vehicle = new Vehicle(1, "Toyota", "Corolla", 2023, "ABC123", 1, "CO");
        
        var result = vehicle.IsEnabledForMarket("US");
        
        result.Should().BeFalse();
    }

    [Fact]
    public void Reserve_WhenVehicleIsAvailable_ShouldChangeStatusToReserved()
    {        
        var vehicle = new Vehicle(1, "Toyota", "Corolla", 2023, "ABC123", 1, "CO");
        
        vehicle.Reserve();
        
        vehicle.Status.Should().Be(VehicleStatus.Reserved);
        vehicle.IsAvailable().Should().BeFalse();
    }

    [Fact]
    public void Reserve_WhenVehicleIsNotAvailable_ShouldThrowInvalidOperationException()
    {        
        var vehicle = new Vehicle(1, "Toyota", "Corolla", 2023, "ABC123", 1, "CO");
        vehicle.Reserve();
        
        Action act = () => vehicle.Reserve();
        
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("El vehículo no está disponible para reserva");
    }

    [Fact]
    public void MakeAvailable_ShouldChangeStatusToAvailable()
    {        
        var vehicle = new Vehicle(1, "Toyota", "Corolla", 2023, "ABC123", 1, "CO");
        vehicle.Reserve();
        
        vehicle.MakeAvailable();
        
        vehicle.Status.Should().Be(VehicleStatus.Available);
        vehicle.IsAvailable().Should().BeTrue();
    }
}