using FleetLedger.Api.Options;
using Microsoft.Extensions.Options;

namespace FleetLedger.Api.Tests.Options;

public class CalendarOptionsValidatorTests
{
    private readonly CalendarOptionsValidator _validator = new();

    [Fact]
    public void Validate_WithTheDefaultTimeZone_Succeeds()
        => _validator.Validate(null, new CalendarOptions()).Succeeded.Should().BeTrue();

    [Fact]
    public void Validate_WithUnknownTimeZone_FailsNamingTheKeyAndTheValue()
    {
        ValidateOptionsResult result = _validator.Validate(null, new CalendarOptions { TimeZone = "Mars/Olympus" });

        result.Failed.Should().BeTrue();
        result.FailureMessage.Should().Contain("Calendar:TimeZone").And.Contain("Mars/Olympus");
    }
}
