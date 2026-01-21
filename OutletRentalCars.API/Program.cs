using Microsoft.EntityFrameworkCore;
using OutletRentalCars.Application.Commands;
using OutletRentalCars.Application.EventHandlers;
using OutletRentalCars.Domain.Interfaces;
using OutletRentalCars.Infrastructure.MongoDB;
using OutletRentalCars.Infrastructure.Persistence;
using OutletRentalCars.Infrastructure.Persistence.Repositories;
using OutletRentalCars.Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "API de alquiler de coches",
        Version = "v1",
        Description = "Sistema de búsqueda y reserva de vehículos",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "outlet By miles CAR RENTAL - Sitio web",
            Url = new Uri("https://www.outletrentalcars.com")
        }
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("OutletRentalCarsDb"));

builder.Services.AddSingleton<MongoDbContext>(sp =>
{
    var connectionString = "mongodb://localhost:27017";
    var databaseName = "OutletRentalCarsConfig";
    return new MongoDbContext(connectionString, databaseName);
});

builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IMarketRepository, MarketRepository>();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateReservationCommand).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(VehicleReservedEventHandler).Assembly);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => 
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var sqlContext = services.GetRequiredService<ApplicationDbContext>();
        var mongoContext = services.GetRequiredService<MongoDbContext>();

        await DataSeeder.SeedAsync(sqlContext, mongoContext);

        Console.WriteLine("Datos sembrados exitosamente");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al sembrar datos: {ex.Message}");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

Console.WriteLine("API ejecutando");
Console.WriteLine("Swagger disponible: https://localhost:{PORT}");

app.Run();

public partial class Program { }