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
        builder.Services.Configure<JsonOptions>(ConfigureJson);
        builder.Services.AddProblemDetails(options =>
            options.CustomizeProblemDetails = context =>
                context.ProblemDetails.Extensions[TraceIdExtension] =
                    Activity.Current?.TraceId.ToString() ?? context.HttpContext.TraceIdentifier);
    }

    // Strict: o padrão web aceita número em string, o que tipa o contrato como
    // number | string para todo cliente gerado do OpenAPI.
    private static void ConfigureJson(JsonOptions options)
    {
        options.SerializerOptions.NumberHandling = JsonNumberHandling.Strict;
        options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }

    // O documento OpenAPI é o contrato com qualquer cliente: publicado em todo ambiente.
    public static void MapHttpContract(this WebApplication app)
        => app.MapOpenApi().AllowAnonymous();
}
