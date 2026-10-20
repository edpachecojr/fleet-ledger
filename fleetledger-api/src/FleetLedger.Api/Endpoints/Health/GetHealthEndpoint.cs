using FleetLedger.Api.Common.Api;
using FleetLedger.Api.Common.Health;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FleetLedger.Api.Endpoints.Health;

// Endpoint Minimal API, e não MapHealthChecks, para entrar no documento OpenAPI:
// os clientes geram o tipo da resposta a partir dele.
public sealed class GetHealthEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet(HealthCheckExtension.HealthPath, HandleAsync)
            .WithName("Health: Get")
            .WithTags("Health")
            .WithSummary("Informa a saúde da API e de suas dependências")
            .WithDescription("Responde 503 quando alguma dependência, como o banco, está inacessível.")
            .Produces<HealthCheckResponse>(StatusCodes.Status200OK)
            .Produces<HealthCheckResponse>(StatusCodes.Status503ServiceUnavailable)
            .AllowAnonymous();

    private static async Task<IResult> HandleAsync(HealthCheckService healthCheckService, CancellationToken cancellationToken)
    {
        HealthReport report = await healthCheckService.CheckHealthAsync(cancellationToken);
        HealthCheckResponse response = HealthCheckResponse.FromReport(report);

        return TypedResults.Json(response, statusCode: response.HttpStatusCode);
    }
}
