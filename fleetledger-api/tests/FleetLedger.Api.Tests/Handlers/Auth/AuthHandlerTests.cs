using FleetLedger.Api.Common.Security;
using FleetLedger.Api.Handlers.Auth;
using FleetLedger.Api.Tests.Fakes;
using FleetLedger.Core.Requests.Auth;
using FleetLedger.Core.Responses;
using FleetLedger.Core.Responses.Auth;
using Microsoft.Extensions.Logging.Abstractions;

namespace FleetLedger.Api.Tests.Handlers.Auth;

public class AuthHandlerTests
{
    private readonly Guid _userId = Guid.CreateVersion7();
    private readonly FakeAccessTokenIssuer _issuer = new();
    private readonly LoginRequest _request = new() { Email = "gestor@fleetledger.dev", Password = "Senha-Forte-123" };

    [Fact]
    public async Task LoginAsync_WithVerifiedCredentials_ReturnsAccessTokenForTheUser()
    {
        Response<IssuedAccessToken?> result = await HandlerWith(CredentialVerification.Verified(_userId))
            .LoginAsync(_request, CancellationToken.None);

        result.Data.Should().BeEquivalentTo(new { AccessToken = $"fake-token-for-{_userId}" });
    }

    [Theory]
    [InlineData(ELoginRefusal.UnknownEmail)]
    [InlineData(ELoginRefusal.WrongPassword)]
    [InlineData(ELoginRefusal.LockedOut)]
    public async Task LoginAsync_WhenRefused_ReturnsTheSameGeneric401(ELoginRefusal refusal)
    {
        Response<IssuedAccessToken?> result = await HandlerWith(CredentialVerification.Refused(refusal))
            .LoginAsync(_request, CancellationToken.None);

        result.Should().BeEquivalentTo(new { Code = 401, Message = AuthResponses.InvalidCredentialsMessage, Data = (object?)null });
    }

    [Fact]
    public async Task LoginAsync_WhenRefused_DoesNotIssueToken()
    {
        await HandlerWith(CredentialVerification.Refused(ELoginRefusal.WrongPassword))
            .LoginAsync(_request, CancellationToken.None);

        _issuer.IssuedFor.Should().BeEmpty();
    }

    private AuthHandler HandlerWith(CredentialVerification verification)
        => new(new FakeCredentialVerifier(verification), _issuer, NullLogger<AuthHandler>.Instance);
}
