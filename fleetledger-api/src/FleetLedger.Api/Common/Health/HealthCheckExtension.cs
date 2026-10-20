using FleetLedger.Api.Data;

namespace FleetLedger.Api.Common.Health;

public static class HealthCheckExtension
{
    public const string HealthPath = "/health";
    public const string DatabaseCheckName = "database";

    public static void AddHealthMonitoring(this WebApplicationBuilder builder)
        => builder.Services
            .AddHealthChecks()
            .AddDbContextCheck<AppDbContext>(DatabaseCheckName);
}
