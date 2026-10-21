using FleetLedger.Api.Handlers;
using FleetLedger.Api.Tests.Fakes;
using FleetLedger.Api.Tests.Fixtures;
using FleetLedger.Core.Models;

namespace FleetLedger.Api.Tests.Handlers;

public class ActivityChangeTests
{
    private readonly DateTimeOffset _now = FakeTimeProvider.ReferenceNow;

    [Fact]
    public void TryApply_DeactivatingAnActiveRecord_StampsDeactivationAndUpdate()
    {
        Vehicle vehicle = VehicleFactory.Create(Guid.CreateVersion7());

        bool changed = ActivityChange.TryApply(vehicle, deactivate: true, _now);

        changed.Should().BeTrue();
        vehicle.Should().BeEquivalentTo(new { DeactivatedAt = (DateTimeOffset?)_now, UpdatedAt = _now, IsActive = false });
    }

    [Fact]
    public void TryApply_ReactivatingAnInactiveRecord_ClearsTheDeactivation()
    {
        Vehicle vehicle = VehicleFactory.Create(Guid.CreateVersion7(), deactivatedAt: _now.AddDays(-3));

        bool changed = ActivityChange.TryApply(vehicle, deactivate: false, _now);

        changed.Should().BeTrue();
        vehicle.IsActive.Should().BeTrue();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void TryApply_ToTheCurrentState_ChangesNothing(bool deactivate)
    {
        Vehicle vehicle = VehicleFactory.Create(Guid.CreateVersion7(), deactivatedAt: deactivate ? _now.AddDays(-3) : null);
        DateTimeOffset updatedAt = vehicle.UpdatedAt;

        bool changed = ActivityChange.TryApply(vehicle, deactivate, _now);

        changed.Should().BeFalse();
        vehicle.UpdatedAt.Should().Be(updatedAt);
    }
}
