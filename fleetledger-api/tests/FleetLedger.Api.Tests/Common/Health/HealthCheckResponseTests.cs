using FleetLedger.Api.Common.Health;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FleetLedger.Api.Tests.Common.Health;

public class HealthCheckResponseTests
{
    [Fact]
    public void FromReport_WhenDatabaseIsUnhealthy_ReportsStatusPerCheck()
    {
        HealthCheckResponse response = HealthCheckResponse.FromReport(ReportWithDatabase(HealthStatus.Unhealthy));

        response.Status.Should().Be(HealthStatus.Unhealthy);
        response.TotalDurationMs.Should().Be(15);
        response.Checks.Should().ContainSingle()
            .Which.Should().Be(new HealthCheckEntryResponse("database", HealthStatus.Unhealthy, 12, "sem conexão"));
    }

    [Theory]
    [InlineData(HealthStatus.Healthy, StatusCodes.Status200OK)]
    [InlineData(HealthStatus.Degraded, StatusCodes.Status200OK)]
    [InlineData(HealthStatus.Unhealthy, StatusCodes.Status503ServiceUnavailable)]
    public void HttpStatusCode_ForEachStatus_IsServiceUnavailableOnlyWhenUnhealthy(HealthStatus status, int expected)
        => HealthCheckResponse.FromReport(ReportWithDatabase(status)).HttpStatusCode.Should().Be(expected);

    private static HealthReport ReportWithDatabase(HealthStatus status)
        => new(
            new Dictionary<string, HealthReportEntry>
            {
                ["database"] = new(status, "sem conexão", TimeSpan.FromMilliseconds(12), null, null)
            },
            TimeSpan.FromMilliseconds(15));
}
