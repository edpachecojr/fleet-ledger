using FleetLedger.Api.Common.Health;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FleetLedger.Api.Tests.Common.Health;

public class HealthCheckResponseTests
{
    [Fact]
    public void FromReport_WhenDatabaseIsUnhealthy_ReportsStatusPerCheck()
    {
        HealthReport report = new(
            new Dictionary<string, HealthReportEntry>
            {
                ["database"] = new(HealthStatus.Unhealthy, "sem conexão", TimeSpan.FromMilliseconds(12), null, null)
            },
            TimeSpan.FromMilliseconds(15));

        HealthCheckResponse response = HealthCheckResponse.FromReport(report);

        response.Status.Should().Be("Unhealthy");
        response.TotalDurationMs.Should().Be(15);
        response.Checks.Should().ContainSingle()
            .Which.Should().Be(new HealthCheckEntryResponse("database", "Unhealthy", 12, "sem conexão"));
    }
}
