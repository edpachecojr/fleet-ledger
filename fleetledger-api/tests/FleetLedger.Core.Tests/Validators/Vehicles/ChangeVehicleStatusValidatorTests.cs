using FleetLedger.Core.Common.Validation;
using FleetLedger.Core.Models;
using FleetLedger.Core.Requests.Vehicles;
using FleetLedger.Core.Validators.Vehicles;

namespace FleetLedger.Core.Tests.Validators.Vehicles;

public class ChangeVehicleStatusValidatorTests
{
    private readonly ChangeVehicleStatusValidator _validator = new();

    [Theory]
    [InlineData(EVehicleStatus.Available)]
    [InlineData(EVehicleStatus.InMaintenance)]
    public void Validate_ForManualTargets_ReturnsValid(EVehicleStatus status)
        => _validator.Validate(new ChangeVehicleStatusRequest { Status = status }).IsValid.Should().BeTrue();

    [Fact]
    public void Validate_WhenTargetIsOnTrip_ReturnsStatusErrorNamingTheValue()
        => _validator.Validate(new ChangeVehicleStatusRequest { Status = EVehicleStatus.OnTrip }).Errors
            .Should().ContainSingle().Which.Should().Match<ValidationError>(
                error => error.Field == "Status" && error.Message.Contains("OnTrip"));

    [Fact]
    public void Validate_WhenStatusIsUndefined_ReturnsStatusError()
        => _validator.Validate(new ChangeVehicleStatusRequest { Status = 0 }).Errors
            .Should().ContainSingle().Which.Field.Should().Be("Status");
}
