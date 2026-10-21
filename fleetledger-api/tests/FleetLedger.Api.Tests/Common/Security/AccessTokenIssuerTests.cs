using FleetLedger.Api.Common.Security;
using FleetLedger.Api.Options;
using FleetLedger.Api.Tests.Fakes;
using FleetLedger.Core.Responses.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using OptionsFactory = Microsoft.Extensions.Options.Options;

namespace FleetLedger.Api.Tests.Common.Security;

public class AccessTokenIssuerTests
{
    private static readonly JwtOptions Jwt = new()
    {
        Issuer = "fleetledger-api",
        Audience = "fleetledger-clients",
        SigningKey = "0123456789abcdef0123456789abcdef",
        AccessTokenLifetimeMinutes = 480
    };

    private readonly Guid _userId = Guid.CreateVersion7();
    private readonly AccessTokenIssuer _issuer = new(OptionsFactory.Create(Jwt), new FakeTimeProvider());

    [Fact]
    public void Issue_ExpiresAfterConfiguredLifetime()
        => _issuer.Issue(_userId).ExpiresAt.Should().Be(FakeTimeProvider.ReferenceNow.AddHours(8));

    [Fact]
    public async Task Issue_ProducesTokenAcceptedByBearerValidation()
    {
        IssuedAccessToken issued = _issuer.Issue(_userId);

        TokenValidationResult result = await new JsonWebTokenHandler()
            .ValidateTokenAsync(issued.AccessToken, BearerValidationWithoutClock());

        result.IsValid.Should().BeTrue(result.Exception?.Message);
    }

    [Fact]
    public void Issue_PutsUserIdInSubjectAndExpirationInToken()
    {
        IssuedAccessToken issued = _issuer.Issue(_userId);

        JsonWebToken token = new(issued.AccessToken);

        token.Subject.Should().Be(_userId.ToString());
        token.ValidTo.Should().Be(issued.ExpiresAt.UtcDateTime);
    }

    // Mesmos parâmetros da API, sem checar validade contra o relógio real
    // (o emissor usa o FakeTimeProvider); a expiração tem teste próprio.
    private static TokenValidationParameters BearerValidationWithoutClock()
    {
        JwtBearerOptions options = new();
        new JwtBearerOptionsSetup(OptionsFactory.Create(Jwt)).Configure(options);
        options.TokenValidationParameters.ValidateLifetime = false;
        return options.TokenValidationParameters;
    }
}
