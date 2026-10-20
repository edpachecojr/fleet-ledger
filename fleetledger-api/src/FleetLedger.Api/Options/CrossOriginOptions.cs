namespace FleetLedger.Api.Options;

public sealed class CrossOriginOptions
{
    public const string SectionName = "Cors";

    // Vazio é válido: cliente que não roda em navegador não precisa de CORS.
    public IReadOnlyList<string> AllowedOrigins { get; init; } = [];
}
