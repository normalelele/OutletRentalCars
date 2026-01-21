using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OutletRentalCars.Domain.Entities;

namespace OutletRentalCars.Infrastructure.Persistence.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .ValueGeneratedOnAdd();

        builder.Property(v => v.Brand)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(v => v.Model)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(v => v.LicensePlate)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(v => v.MarketCode)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(v => v.Status)
            .IsRequired()
            .HasConversion<string>();
        
        builder.HasOne(v => v.Location)
            .WithMany()
            .HasForeignKey(v => v.LocationId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Ignore(v => v.DomainEvents);
    }
}