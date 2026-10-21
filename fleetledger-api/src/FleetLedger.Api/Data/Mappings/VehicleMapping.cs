using FleetLedger.Core.Models;
using FleetLedger.Core.Validators.Vehicles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetLedger.Api.Data.Mappings;

public sealed class VehicleMapping : IEntityTypeConfiguration<Vehicle>
{
    // R$ com centavos (decisão de 2026-10-21); 8 inteiros cobrem qualquer taxa realista.
    private const int RatePrecision = 10;

    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("vehicles");
        builder.Property(vehicle => vehicle.Plate).HasMaxLength(VehiclePlate.Length).IsRequired();
        builder.Property(vehicle => vehicle.Model).HasMaxLength(VehicleSpecificationRules.ModelMaxLength).IsRequired();
        builder.Property(vehicle => vehicle.MaintenanceProvisionRatePerKm)
            .HasPrecision(RatePrecision, VehicleSpecificationRules.RateDecimalPlaces);
        builder.Property(vehicle => vehicle.DeactivatedAt).HasPrecision(6);

        // Placa única por organização, inclusive entre inativos (TASK-004).
        builder.HasIndex(vehicle => new { vehicle.OrganizationId, vehicle.Plate }).IsUnique();
    }
}
