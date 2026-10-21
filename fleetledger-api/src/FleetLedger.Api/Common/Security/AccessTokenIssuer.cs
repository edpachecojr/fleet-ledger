using System.Security.Claims;
using FleetLedger.Api.Options;
using FleetLedger.Core.Responses.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace FleetLedger.Api.Common.Security;

// Emite o JWT validado por JwtBearerOptionsSetup. Só o id do usuário vai no token:
// a organização é resolvida no servidor por IUserContext (docs/adr/0003) e nenhum
// e-mail (PII) trafega em claim.
public sealed class AccessTokenIssuer(IOptions<JwtOptions> jwtOptions, TimeProvider timeProvider) : IAccessTokenIssuer
{
    private readonly JsonWebTokenHandler _tokenHandler = new();

    public IssuedAccessToken Issue(Guid userId)
    {
        JwtOptions jwt = jwtOptions.Value;
        DateTimeOffset issuedAt = timeProvider.GetUtcNow();
        DateTimeOffset expiresAt = issuedAt.Add(jwt.AccessTokenLifetime);

        string accessToken = _tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = jwt.Issuer,
            Audience = jwt.Audience,
            Subject = new ClaimsIdentity(SubjectClaims(userId)),
            IssuedAt = issuedAt.UtcDateTime,
            NotBefore = issuedAt.UtcDateTime,
            Expires = expiresAt.UtcDateTime,
            SigningCredentials = new SigningCredentials(JwtSigningKey.From(jwt), SecurityAlgorithms.HmacSha256)
        });

        return new IssuedAccessToken(accessToken, expiresAt);
    }

    private static Claim[] SubjectClaims(Guid userId)
        =>
        [
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.CreateVersion7().ToString())
        ];
}
