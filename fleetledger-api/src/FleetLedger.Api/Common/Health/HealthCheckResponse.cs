using System.Text.Json.Serialization;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FleetLedger.Api.Common.Health;

public sealed record HealthCheckResponse(
    HealthStatus Status,
    double TotalDurationMs,
    IReadOnlyList<HealthCheckEntryResponse> Checks)
{
    // Exceção e dados internos ficam fora: o endpoint é anônimo.
    public static HealthCheckResponse FromReport(HealthReport report)
        => new(
            report.Status,
            report.TotalDuration.TotalMilliseconds,
            [.. report.Entries.Select(entry => new HealthCheckEntryResponse(
                entry.Key,
                entry.Value.Status,
                entry.Value.Duration.TotalMilliseconds,
                entry.Value.Description))]);

    // Degraded continua 200: a API responde, só com alguma dependência lenta.
    [JsonIgnore]
    public int HttpStatusCode
        => Status == HealthStatus.Unhealthy ? StatusCodes.Status503ServiceUnavailable : StatusCodes.Status200OK;
}
