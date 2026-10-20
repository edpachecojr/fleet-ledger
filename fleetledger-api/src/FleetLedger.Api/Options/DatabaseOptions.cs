namespace FleetLedger.Api.Options;

public sealed class DatabaseOptions
{
    public const string SectionName = "Database";

    public string ConnectionString { get; init; } = string.Empty;

    // Ligado em desenvolvimento e no docker compose; em produção as migrations
    // rodam como etapa de deploy, não a cada réplica que sobe.
    public bool ApplyMigrationsOnStartup { get; init; }
}
