using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FleetLedger.Api.Common.Health;

public sealed record HealthCheckResponse(
    string Status,
    double TotalDurationMs,
    IReadOnlyList<HealthCheckEntryResponse> Checks)
{
    // Exceção e dados internos ficam fora: o endpoint é anônimo.
    public static HealthCheckResponse FromReport(HealthReport report)
        => new(
            report.Status.ToString(),
            report.TotalDuration.TotalMilliseconds,
            [.. report.Entries.Select(entry => new HealthCheckEntryResponse(
                entry.Key,
                entry.Value.Status.ToString(),
                entry.Value.Duration.TotalMilliseconds,
                entry.Value.Description))]);
}
