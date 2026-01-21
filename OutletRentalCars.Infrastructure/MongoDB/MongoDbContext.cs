using MongoDB.Driver;
using OutletRentalCars.Domain.Entities;

namespace OutletRentalCars.Infrastructure.MongoDB;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<Market> Markets =>
        _database.GetCollection<Market>("markets");
}