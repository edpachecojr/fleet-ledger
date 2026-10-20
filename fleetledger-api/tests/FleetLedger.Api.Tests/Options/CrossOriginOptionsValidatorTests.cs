using FleetLedger.Api.Options;
using Microsoft.Extensions.Options;

namespace FleetLedger.Api.Tests.Options;

public class CrossOriginOptionsValidatorTests
{
    private readonly CrossOriginOptionsValidator _validator = new();

    [Fact]
    public void Validate_WhenNoOriginIsConfigured_Succeeds()
        => _validator.Validate(null, new CrossOriginOptions()).Succeeded.Should().BeTrue();

    [Theory]
    [InlineData("http://localhost:5173")]
    [InlineData("https://app.fleetledger.example")]
    public void Validate_WhenOriginIsAbsoluteHttp_Succeeds(string origin)
        => _validator.Validate(null, WithOrigins(origin)).Succeeded.Should().BeTrue();

    [Theory]
    [InlineData("*")]
    [InlineData("localhost:5173")]
    [InlineData("ftp://files.fleetledger.example")]
    [InlineData("https://app.fleetledger.example/")]
    [InlineData("https://app.fleetledger.example/login")]
    public void Validate_WhenOriginIsInvalid_FailsWithIndexAndValue(string origin)
    {
        ValidateOptionsResult result = _validator.Validate(null, WithOrigins("http://localhost:5173", origin));

        result.FailureMessage.Should().Contain("Cors:AllowedOrigins:1").And.Contain(origin);
    }

    private static CrossOriginOptions WithOrigins(params string[] origins) => new() { AllowedOrigins = origins };
}
