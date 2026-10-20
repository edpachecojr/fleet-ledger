using FleetLedger.Api.Options;
using Microsoft.Extensions.Options;

namespace FleetLedger.Api.Tests.Options;

public class JwtOptionsValidatorTests
{
    private const string ValidSigningKey = "0123456789abcdef0123456789abcdef";

    private readonly JwtOptionsValidator _validator = new();

    [Fact]
    public void Validate_WhenAllValuesAreSet_Succeeds()
        => _validator.Validate(null, ValidOptions()).Succeeded.Should().BeTrue();

    [Fact]
    public void Validate_WhenIssuerIsBlank_FailsNamingTheKey()
        => _validator.Validate(null, ValidOptions(issuer: "")).FailureMessage.Should().Contain("Jwt:Issuer");

    [Fact]
    public void Validate_WhenAudienceIsBlank_FailsNamingTheKey()
        => _validator.Validate(null, ValidOptions(audience: "")).FailureMessage.Should().Contain("Jwt:Audience");

    [Fact]
    public void Validate_WhenSigningKeyIsShorterThan32Bytes_FailsWithReceivedLengthButNotTheKey()
    {
        ValidateOptionsResult result = _validator.Validate(null, ValidOptions(signingKey: "short-secret"));

        result.FailureMessage.Should().Contain("Jwt:SigningKey").And.Contain("12 bytes");
        result.FailureMessage.Should().NotContain("short-secret");
    }

    private static JwtOptions ValidOptions(
        string issuer = "fleetledger-api",
        string audience = "fleetledger-clients",
        string signingKey = ValidSigningKey)
        => new() { Issuer = issuer, Audience = audience, SigningKey = signingKey };
}
