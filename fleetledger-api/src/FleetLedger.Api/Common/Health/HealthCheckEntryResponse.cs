using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FleetLedger.Api.Common.Health;

public sealed record HealthCheckEntryResponse(string Name, HealthStatus Status, double DurationMs, string? Description);
