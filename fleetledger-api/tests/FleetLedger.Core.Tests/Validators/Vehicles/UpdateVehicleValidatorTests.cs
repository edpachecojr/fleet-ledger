using FleetLedger.Core.Requests.Vehicles;
using FleetLedger.Core.Validators.Vehicles;

namespace FleetLedger.Core.Tests.Validators.Vehicles;

public class UpdateVehicleValidatorTests
{
    private readonly UpdateVehicleValidator _validator = new();

    [Fact]
    public void Validate_WhenEditableFieldsAreValid_ReturnsValid()
        => _validator.Validate(ValidRequest()).IsValid.Should().BeTrue();

    [Fact]
    public void Validate_WhenModelIsBlank_ReturnsModelError()
    {
        UpdateVehicleRequest request = ValidRequest();
        request.Model = "";

        _validator.Validate(request).Errors.Should().ContainSingle().Which.Field.Should().Be("Model");
    }

    [Fact]
    public void Validate_WhenCapacityIsZero_ReturnsCapacityError()
    {
        UpdateVehicleRequest request = ValidRequest();
        request.CapacityKg = 0;

        _validator.Validate(request).Errors.Should().ContainSingle().Which.Field.Should().Be("CapacityKg");
    }

    [Fact]
    public void Validate_WhenRateIsNegative_ReturnsRateError()
    {
        UpdateVehicleRequest request = ValidRequest();
        request.MaintenanceProvisionRatePerKm = -1m;

        _validator.Validate(request).Errors.Should().ContainSingle().Which.Field.Should().Be("MaintenanceProvisionRatePerKm");
    }

    private static UpdateVehicleRequest ValidRequest()
        => new() { Model = "Scania R 450", CapacityKg = 30000, MaintenanceProvisionRatePerKm = 0.2m };
}
