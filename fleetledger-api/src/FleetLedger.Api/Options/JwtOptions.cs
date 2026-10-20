namespace FleetLedger.Api.Options;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    // HS256 exige chave de no mínimo 256 bits.
    public const int MinimumSigningKeyBytes = 32;

    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public string SigningKey { get; init; } = string.Empty;
}
