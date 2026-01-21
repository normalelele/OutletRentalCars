using MongoDB.Driver;
using OutletRentalCars.Domain.Entities;
using OutletRentalCars.Infrastructure.MongoDB;
using OutletRentalCars.Infrastructure.Persistence;

namespace OutletRentalCars.Infrastructure.Seed;

public static class DataSeeder
{
    public static async Task SeedAsync(ApplicationDbContext sqlContext, MongoDbContext mongoContext)
    {       
        await SeedLocationsAsync(sqlContext);
        await SeedVehiclesAsync(sqlContext);
        
        await SeedMarketsAsync(mongoContext);
    }

    private static async Task SeedLocationsAsync(ApplicationDbContext context)
    {
        if (context.Locations.Any()) return;

        var locations = new List<Location>
        {
            new Location(1, "Bogotá Airport", "Bogotá", "CO"),
            new Location(2, "Medellín Downtown", "Medellín", "CO"),
            new Location(3, "Cartagena Beach", "Cartagena", "CO"),
            new Location(4, "Miami Airport", "Miami", "US"),
            new Location(5, "New York JFK", "New York", "US")
        };

        await context.Locations.AddRangeAsync(locations);
        await context.SaveChangesAsync();
    }

    private static async Task SeedVehiclesAsync(ApplicationDbContext context)
    {
        if (context.Vehicles.Any()) return;

        var vehicles = new List<Vehicle>
        {
            new Vehicle(1, "Toyota", "Corolla", 2023, "ABC123", 1, "CO"),
            new Vehicle(2, "Chevrolet", "Spark", 2024, "DEF456", 1, "CO"),
            new Vehicle(3, "Mazda", "CX-5", 2023, "GHI789", 2, "CO"),
            new Vehicle(4, "Renault", "Logan", 2024, "JKL012", 2, "CO"),
            new Vehicle(5, "Ford", "Escape", 2023, "MNO345", 3, "CO"),
            new Vehicle(6, "Honda", "Civic", 2024, "PQR678", 4, "US"),
            new Vehicle(7, "Tesla", "Model 3", 2024, "STU901", 4, "US"),
            new Vehicle(8, "BMW", "X5", 2023, "VWX234", 5, "US")
        };

        await context.Vehicles.AddRangeAsync(vehicles);
        await context.SaveChangesAsync();
    }

    private static async Task SeedMarketsAsync(MongoDbContext context)
    {
        var filter = Builders<Market>.Filter.Empty;
        var existingMarkets = await context.Markets.CountDocumentsAsync(filter);
        if (existingMarkets > 0) return;

        var markets = new List<Market>
        {
            new Market("CO", "Colombia", true),
            new Market("US", "United States", true),
            new Market("MX", "Mexico", true),
            new Market("BR", "Brazil", false)
        };

        await context.Markets.InsertManyAsync(markets);
    }
}