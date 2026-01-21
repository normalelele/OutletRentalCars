using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OutletRentalCars.Domain.Entities;

namespace OutletRentalCars.Infrastructure.Persistence.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .ValueGeneratedOnAdd();

        builder.Property(r => r.CustomerName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(r => r.CustomerEmail)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(r => r.PickupDateTime)
            .IsRequired();

        builder.Property(r => r.ReturnDateTime)
            .IsRequired();

        builder.Property(r => r.IsActive)
            .IsRequired();
        
        builder.HasOne(r => r.Vehicle)
            .WithMany()
            .HasForeignKey(r => r.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.PickupLocation)
            .WithMany()
            .HasForeignKey(r => r.PickupLocationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.ReturnLocation)
            .WithMany()
            .HasForeignKey(r => r.ReturnLocationId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Ignore(r => r.DomainEvents);
    }
}