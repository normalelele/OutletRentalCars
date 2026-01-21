using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;

namespace OutletRentalCars.Tests.IntegrationTests;

public class VehicleSearchIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public VehicleSearchIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task SearchVehicles_WithValidParameters_ShouldReturnOk()
    {        
        var pickupLocationId = 1;
        var returnLocationId = 2;
        var pickupDate = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ss");
        var returnDate = DateTime.UtcNow.AddDays(5).ToString("yyyy-MM-ddTHH:mm:ss");

        var url = $"/api/vehicles/search?pickupLocationId={pickupLocationId}" +
                  $"&returnLocationId={returnLocationId}" +
                  $"&pickupDateTime={pickupDate}" +
                  $"&returnDateTime={returnDate}";
        
        var response = await _client.GetAsync(url);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task SearchVehicles_WithInvalidDates_ShouldReturnBadRequest()
    {        
        var pickupLocationId = 1;
        var returnLocationId = 2;
        var pickupDate = DateTime.UtcNow.AddDays(10).ToString("yyyy-MM-ddTHH:mm:ss");
        var returnDate = DateTime.UtcNow.AddDays(5).ToString("yyyy-MM-ddTHH:mm:ss");

        var url = $"/api/vehicles/search?pickupLocationId={pickupLocationId}" +
                  $"&returnLocationId={returnLocationId}" +
                  $"&pickupDateTime={pickupDate}" +
                  $"&returnDateTime={returnDate}";
        
        var response = await _client.GetAsync(url);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task SearchVehicles_WithNonExistentLocation_ShouldReturnBadRequest()
    {        
        var pickupLocationId = 9999;
        var returnLocationId = 2;
        var pickupDate = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ss");
        var returnDate = DateTime.UtcNow.AddDays(5).ToString("yyyy-MM-ddTHH:mm:ss");

        var url = $"/api/vehicles/search?pickupLocationId={pickupLocationId}" +
                  $"&returnLocationId={returnLocationId}" +
                  $"&pickupDateTime={pickupDate}" +
                  $"&returnDateTime={returnDate}";
        
        var response = await _client.GetAsync(url);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task SearchVehicles_ShouldReturnVehiclesInCorrectFormat()
    {        
        var pickupLocationId = 1;
        var returnLocationId = 2;
        var pickupDate = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ss");
        var returnDate = DateTime.UtcNow.AddDays(5).ToString("yyyy-MM-ddTHH:mm:ss");

        var url = $"/api/vehicles/search?pickupLocationId={pickupLocationId}" +
                  $"&returnLocationId={returnLocationId}" +
                  $"&pickupDateTime={pickupDate}" +
                  $"&returnDateTime={returnDate}";
        
        var response = await _client.GetAsync(url);
        var result = await response.Content.ReadFromJsonAsync<SearchResult>();
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
    }
    
    private class SearchResult
    {
        public bool Success { get; set; }
        public List<VehicleDto> Data { get; set; }
        public int Count { get; set; }
    }

    private class VehicleDto
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string LicensePlate { get; set; }
        public string Status { get; set; }
        public string MarketCode { get; set; }
    }
}