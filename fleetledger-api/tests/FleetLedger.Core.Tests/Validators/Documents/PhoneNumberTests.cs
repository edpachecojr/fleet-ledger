using FleetLedger.Core.Validators.Documents;

namespace FleetLedger.Core.Tests.Validators.Documents;

public class PhoneNumberTests
{
    [Theory]
    [InlineData("1133334444")]
    [InlineData("11987654321")]
    public void IsValid_WithAreaCodeAndNumber_ReturnsTrue(string phone) => PhoneNumber.IsValid(phone).Should().BeTrue();

    [Theory]
    [InlineData("987654321")]
    [InlineData("119876543210")]
    [InlineData("")]
    public void IsValid_WithoutTenOrElevenDigits_ReturnsFalse(string phone) => PhoneNumber.IsValid(phone).Should().BeFalse();
}
