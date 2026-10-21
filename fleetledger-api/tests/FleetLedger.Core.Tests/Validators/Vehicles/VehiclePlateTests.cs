using FleetLedger.Core.Validators.Vehicles;

namespace FleetLedger.Core.Tests.Validators.Vehicles;

public class VehiclePlateTests
{
    [Theory]
    [InlineData("abc-1234", "ABC1234")]
    [InlineData(" abc 1d23 ", "ABC1D23")]
    [InlineData(null, "")]
    public void Normalize_RemovesSeparatorsAndUppercases(string? plate, string expected)
        => VehiclePlate.Normalize(plate).Should().Be(expected);

    [Theory]
    [InlineData("ABC1234")]
    [InlineData("ABC1D23")]
    public void IsValid_ForOldAndMercosulFormats_ReturnsTrue(string plate)
        => VehiclePlate.IsValid(plate).Should().BeTrue();

    [Theory]
    [InlineData("AB12345")]
    [InlineData("ABCD123")]
    [InlineData("ABC12345")]
    [InlineData("ABC1DD3")]
    [InlineData("")]
    public void IsValid_ForOtherShapes_ReturnsFalse(string plate)
        => VehiclePlate.IsValid(plate).Should().BeFalse();
}
