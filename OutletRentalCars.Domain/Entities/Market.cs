using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OutletRentalCars.Domain.Entities;

public class Market
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
        
    [BsonElement("code")]
    public string Code { get; set; }

    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("isActive")]
    public bool IsActive { get; set; }

    public Market(string code, string name, bool isActive = true)
    {
        Code = code;
        Name = name;
        IsActive = isActive;
    }
}