using FleetLedger.Api.Tests.Fakes;
using FleetLedger.Core.Models;

namespace FleetLedger.Api.Tests.Fixtures;

public static class VehicleFactory
{
    public static Vehicle Create(
        Guid organizationId,
        string plate = "ABC1D23",
        EVehicleStatus status = EVehicleStatus.Available,
        DateTimeOffset? deactivatedAt = null)
        => new()
        {
            OrganizationId = organizationId,
            Plate = plate,
            Model = "Volvo FH 540",
            CapacityKg = 27000,
            InitialOdometerKm = 100000,
            CurrentOdometerKm = 125000,
            MaintenanceProvisionRatePerKm = 0.15m,
            Status = status,
            DeactivatedAt = deactivatedAt,
            CreatedAt = FakeTimeProvider.ReferenceNow.AddDays(-30),
            UpdatedAt = FakeTimeProvider.ReferenceNow.AddDays(-30)
        };
}
