namespace FleetLedger.Api.Common.Health;

public sealed record HealthCheckEntryResponse(string Name, string Status, double DurationMs, string? Description);
