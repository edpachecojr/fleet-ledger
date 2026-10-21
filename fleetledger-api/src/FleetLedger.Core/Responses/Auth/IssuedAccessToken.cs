namespace FleetLedger.Core.Responses.Auth;

/// <summary>
/// Access token (JWT) emitido no login. Não há refresh token: ao expirar, o cliente pede novo login.
/// </summary>
/// <example><code>new IssuedAccessToken("eyJhbGciOi...", now.AddHours(8))</code></example>
/// <param name="AccessToken">JWT a enviar no header <c>Authorization: Bearer</c>.</param>
/// <param name="ExpiresAt">Momento (UTC) a partir do qual a API recusa o token.</param>
public sealed record IssuedAccessToken(string AccessToken, DateTimeOffset ExpiresAt);
