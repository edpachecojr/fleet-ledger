using FleetLedger.Api.Data;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FleetLedger.Api.Common.Health;

public static class HealthCheckExtension
{
    public const string HealthPath = "/health";
    public const string DatabaseCheckName = "database";

    public static void AddHealthMonitoring(this WebApplicationBuilder builder)
        => builder.Services
            .AddHealthChecks()
            .AddDbContextCheck<AppDbContext>(DatabaseCheckName);

    // Responde 503 quando o banco está inacessível, com o detalhe por verificação.
    public static void MapHealthEndpoint(this IEndpointRouteBuilder app)
        => app.MapHealthChecks(HealthPath, new HealthCheckOptions { ResponseWriter = WriteResponseAsync })
            .AllowAnonymous();

    private static Task WriteResponseAsync(HttpContext context, HealthReport report)
        => context.Response.WriteAsJsonAsync(HealthCheckResponse.FromReport(report), context.RequestAborted);
}
