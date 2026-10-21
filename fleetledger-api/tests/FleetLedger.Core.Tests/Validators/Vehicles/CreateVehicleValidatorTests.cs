using FleetLedger.Core.Requests.Vehicles;
using FleetLedger.Core.Validators.Vehicles;

namespace FleetLedger.Core.Tests.Validators.Vehicles;

public class CreateVehicleValidatorTests
{
    private readonly CreateVehicleValidator _validator = new();

    [Fact]
    public void Validate_WhenEveryFieldIsValid_ReturnsValid()
        => _validator.Validate(ValidRequest()).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("")]
    [InlineData("ABC-12")]
    [InlineData("1BC1234")]
    public void Validate_WhenPlateIsMissingOrMalformed_ReturnsPlateError(string plate)
        => FieldsWithError(Request(r => r.Plate = plate)).Should().ContainSingle().Which.Should().Be("Plate");

    [Fact]
    public void Validate_WhenPlateIsMalformed_MessageNamesTheValueAndTheExpectedFormats()
        => _validator.Validate(Request(r => r.Plate = "XYZ")).Errors.Single().Message
            .Should().Contain("XYZ").And.Contain("Mercosul");

    [Fact]
    public void Validate_WhenModelIsBlank_ReturnsModelError()
        => FieldsWithError(Request(r => r.Model = " ")).Should().ContainSingle().Which.Should().Be("Model");

    [Fact]
    public void Validate_WhenModelIsTooLong_ReturnsModelError()
        => FieldsWithError(Request(r => r.Model = new string('m', VehicleSpecificationRules.ModelMaxLength + 1)))
            .Should().ContainSingle().Which.Should().Be("Model");

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_WhenCapacityIsNotPositive_ReturnsCapacityError(int capacityKg)
        => FieldsWithError(Request(r => r.CapacityKg = capacityKg)).Should().ContainSingle().Which.Should().Be("CapacityKg");

    [Fact]
    public void Validate_WhenInitialOdometerIsNegative_ReturnsOdometerError()
        => FieldsWithError(Request(r => r.InitialOdometerKm = -1)).Should().ContainSingle().Which.Should().Be("InitialOdometerKm");

    [Fact]
    public void Validate_WhenInitialOdometerIsZero_ReturnsValid()
        => _validator.Validate(Request(r => r.InitialOdometerKm = 0)).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("-0.01")]
    [InlineData("0.125")]
    public void Validate_WhenRateIsNegativeOrHasMoreThanTwoDecimals_ReturnsRateError(string rate)
        => FieldsWithError(Request(r => r.MaintenanceProvisionRatePerKm = decimal.Parse(rate, System.Globalization.CultureInfo.InvariantCulture)))
            .Should().ContainSingle().Which.Should().Be("MaintenanceProvisionRatePerKm");

    private IEnumerable<string> FieldsWithError(CreateVehicleRequest request)
        => _validator.Validate(request).Errors.Select(error => error.Field);

    private static CreateVehicleRequest Request(Action<CreateVehicleRequest> change)
    {
        CreateVehicleRequest request = ValidRequest();
        change(request);
        return request;
    }

    private static CreateVehicleRequest ValidRequest()
        => new()
        {
            Plate = "abc-1d23",
            Model = "Volvo FH 540",
            CapacityKg = 27000,
            InitialOdometerKm = 120000,
            MaintenanceProvisionRatePerKm = 0.15m
        };
}
