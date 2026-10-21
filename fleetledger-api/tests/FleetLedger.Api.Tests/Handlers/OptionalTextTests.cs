using FleetLedger.Api.Handlers;

namespace FleetLedger.Api.Tests.Handlers;

public class OptionalTextTests
{
    [Theory]
    [InlineData(null, null)]
    [InlineData("", null)]
    [InlineData("   ", null)]
    [InlineData(" Agro Sul ", "Agro Sul")]
    public void TrimToNull_TreatsBlankAsNotInformed(string? value, string? expected)
        => OptionalText.TrimToNull(value).Should().Be(expected);
}
