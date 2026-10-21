using FleetLedger.Api.Handlers.Vehicles;
using FleetLedger.Api.Tests.Fixtures;
using FleetLedger.Core.Models;

namespace FleetLedger.Api.Tests.Handlers.Vehicles;

public class VehicleAllocationRuleTests
{
    private readonly Guid _organizationId = Guid.CreateVersion7();

    [Fact]
    public void IsAllocatable_WhenActiveAndAvailable_ReturnsTrue()
        => VehicleAllocationRule.IsAllocatable(VehicleFactory.Create(_organizationId)).Should().BeTrue();

    [Theory]
    [InlineData(EVehicleStatus.OnTrip)]
    [InlineData(EVehicleStatus.InMaintenance)]
    public void IsAllocatable_WhenNotAvailable_ReturnsFalse(EVehicleStatus status)
        => VehicleAllocationRule.IsAllocatable(VehicleFactory.Create(_organizationId, status: status)).Should().BeFalse();

    [Fact]
    public void IsAllocatable_WhenInactive_ReturnsFalse()
        => VehicleAllocationRule.IsAllocatable(VehicleFactory.Create(_organizationId, deactivatedAt: DateTimeOffset.UnixEpoch))
            .Should().BeFalse();
}
