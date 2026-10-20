using System.Diagnostics;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;

namespace FleetLedger.Api.Common.Api;

public static class HttpContractExtension
{
    private const string TraceIdExtension = "traceId";

    public static void AddHttpContract(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi();
        builder.Services.Configure<JsonOptions>(options =>
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        builder.Services.AddProblemDetails(options =>
            options.CustomizeProblemDetails = context =>
                context.ProblemDetails.Extensions[TraceIdExtension] =
                    Activity.Current?.TraceId.ToString() ?? context.HttpContext.TraceIdentifier);
    }

    // O documento OpenAPI é o contrato com qualquer cliente: publicado em todo ambiente.
    public static void MapHttpContract(this WebApplication app)
        => app.MapOpenApi().AllowAnonymous();
}
