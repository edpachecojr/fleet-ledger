namespace FleetLedger.Api.Options;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    // HS256 exige chave de no mínimo 256 bits.
    public const int MinimumSigningKeyBytes = 32;

    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public string SigningKey { get; init; } = string.Empty;

    // 8 h cobre um expediente sem refresh token: expirou, novo login (decisão da TASK-002).
    public const int DefaultAccessTokenLifetimeMinutes = 480;
    public const int MinimumAccessTokenLifetimeMinutes = 5;
    public const int MaximumAccessTokenLifetimeMinutes = 1440;

    public int AccessTokenLifetimeMinutes { get; init; } = DefaultAccessTokenLifetimeMinutes;

    public TimeSpan AccessTokenLifetime => TimeSpan.FromMinutes(AccessTokenLifetimeMinutes);
}
