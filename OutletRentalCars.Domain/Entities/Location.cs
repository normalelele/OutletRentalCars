namespace OutletRentalCars.Domain.Entities;

public class Location
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string City { get; private set; }
    public string CountryCode { get; private set; }

    private Location() { }

    public Location(int id, string name, string city, string countryCode)
    {
        Id = id;
        Name = name;
        City = city;
        CountryCode = countryCode;
    }
    
    public bool BelongsToCountry(string countryCode)
    {
        return CountryCode.Equals(countryCode, StringComparison.OrdinalIgnoreCase);
    }
}