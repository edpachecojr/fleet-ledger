using FleetLedger.Core.Validators.Documents;

namespace FleetLedger.Core.Tests.Validators.Documents;

public class DigitsOnlyTests
{
    [Theory]
    [InlineData("11.222.333/0001-81", "11222333000181")]
    [InlineData("(11) 98765-4321", "11987654321")]
    [InlineData(null, "")]
    public void Of_KeepsOnlyAsciiDigits(string? value, string expected) => DigitsOnly.Of(value).Should().Be(expected);
}
