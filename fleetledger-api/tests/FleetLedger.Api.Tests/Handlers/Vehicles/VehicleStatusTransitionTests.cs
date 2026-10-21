using FleetLedger.Api.Handlers.Vehicles;
using FleetLedger.Api.Tests.Fixtures;
using FleetLedger.Core.Models;

namespace FleetLedger.Api.Tests.Handlers.Vehicles;

public class VehicleStatusTransitionTests
{
    private readonly Guid _organizationId = Guid.CreateVersion7();

    [Theory]
    [InlineData(EVehicleStatus.Available, EVehicleStatus.InMaintenance)]
    [InlineData(EVehicleStatus.InMaintenance, EVehicleStatus.Available)]
    public void Evaluate_BetweenAvailableAndMaintenance_IsAllowed(EVehicleStatus from, EVehicleStatus to)
        => VehicleStatusTransition.Evaluate(VehicleFactory.Create(_organizationId, status: from), to)
            .Should().Be(EVehicleStatusChangeVerdict.Allowed);

    [Theory]
    [InlineData(EVehicleStatus.Available)]
    [InlineData(EVehicleStatus.InMaintenance)]
    public void Evaluate_FromOnTrip_IsBlocked(EVehicleStatus target)
        => VehicleStatusTransition.Evaluate(VehicleFactory.Create(_organizationId, status: EVehicleStatus.OnTrip), target)
            .Should().Be(EVehicleStatusChangeVerdict.BlockedOnTrip);

    [Fact]
    public void Evaluate_ForInactiveVehicle_IsBlocked()
        => VehicleStatusTransition.Evaluate(
                VehicleFactory.Create(_organizationId, deactivatedAt: DateTimeOffset.UnixEpoch),
                EVehicleStatus.InMaintenance)
            .Should().Be(EVehicleStatusChangeVerdict.BlockedInactive);

    [Fact]
    public void Evaluate_ToTheCurrentStatus_IsUnchanged()
        => VehicleStatusTransition.Evaluate(VehicleFactory.Create(_organizationId), EVehicleStatus.Available)
            .Should().Be(EVehicleStatusChangeVerdict.Unchanged);
}
