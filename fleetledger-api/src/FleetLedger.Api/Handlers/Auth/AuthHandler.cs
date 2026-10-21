using FleetLedger.Api.Common.Security;
using FleetLedger.Core.Handlers;
using FleetLedger.Core.Requests.Auth;
using FleetLedger.Core.Responses;
using FleetLedger.Core.Responses.Auth;

namespace FleetLedger.Api.Handlers.Auth;

public sealed class AuthHandler(
    ICredentialVerifier credentialVerifier,
    IAccessTokenIssuer accessTokenIssuer,
    ILogger<AuthHandler> logger) : IAuthHandler
{
    public async Task<Response<IssuedAccessToken?>> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        CredentialVerification verification = await credentialVerifier.VerifyAsync(request.Email, request.Password);
        if (!verification.IsVerified)
        {
            logger.LogWarning("Login recusado: {LoginRefusal}", verification.Refusal);
            return AuthResponses.InvalidCredentials();
        }

        IssuedAccessToken accessToken = accessTokenIssuer.Issue(verification.UserId);
        logger.LogInformation(
            "Login do usuário {UserId} aceito; access token expira em {ExpiresAt}",
            verification.UserId,
            accessToken.ExpiresAt);

        return AuthResponses.LoggedIn(accessToken);
    }
}
