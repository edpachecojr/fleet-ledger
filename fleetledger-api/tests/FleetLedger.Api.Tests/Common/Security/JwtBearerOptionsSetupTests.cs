using FleetLedger.Api.Common.Security;
using FleetLedger.Api.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OptionsFactory = Microsoft.Extensions.Options.Options;

namespace FleetLedger.Api.Tests.Common.Security;

public class JwtBearerOptionsSetupTests
{
    private readonly JwtBearerOptionsSetup _setup = new(OptionsFactory.Create(new JwtOptions
    {
        Issuer = "fleetledger-api",
        Audience = "fleetledger-clients",
        SigningKey = "0123456789abcdef0123456789abcdef"
    }));

    [Fact]
    public void Configure_ForBearerScheme_ValidatesIssuerAudienceKeyAndLifetime()
    {
        JwtBearerOptions options = new();

        _setup.Configure(JwtBearerDefaults.AuthenticationScheme, options);

        TokenValidationParameters parameters = options.TokenValidationParameters;
        parameters.ValidIssuer.Should().Be("fleetledger-api");
        parameters.ValidAudience.Should().Be("fleetledger-clients");
        parameters.IssuerSigningKey.Should().BeOfType<SymmetricSecurityKey>();
        parameters.ValidateLifetime.Should().BeTrue();
    }

    [Fact]
    public void Configure_ForAnotherScheme_LeavesOptionsUntouched()
    {
        JwtBearerOptions options = new();

        _setup.Configure("OtherScheme", options);

        options.TokenValidationParameters.ValidIssuer.Should().BeNull();
    }
}
