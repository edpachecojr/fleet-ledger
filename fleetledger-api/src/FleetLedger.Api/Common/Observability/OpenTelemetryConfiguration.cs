using Npgsql;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace FleetLedger.Api.Common.Observability;

public static class OpenTelemetryConfiguration
{
    public static void AddTelemetry(this WebApplicationBuilder builder)
    {
        IOpenTelemetryBuilder telemetry = builder.Services
            .AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(
                serviceName: ObservabilityDefaults.ServiceName,
                serviceVersion: ObservabilityDefaults.ServiceVersion))
            .WithTracing(ConfigureTracing)
            .WithMetrics(ConfigureMetrics);

        AddOtlpExporter(telemetry, builder.Configuration[ObservabilityDefaults.OtlpEndpointKey]);
    }

    private static void ConfigureTracing(TracerProviderBuilder tracing)
        => tracing
            .AddSource(ObservabilityDefaults.ServiceName)
            .AddAspNetCoreInstrumentation(options =>
                options.Filter = httpContext => !httpContext.Request.Path.StartsWithSegments("/health"))
            .AddHttpClientInstrumentation()
            .AddNpgsql();

    private static void ConfigureMetrics(MeterProviderBuilder metrics)
        => metrics
            .AddMeter(ObservabilityDefaults.ServiceName)
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation();

    // Sem endpoint configurado não há coletor para receber: o exportador ficaria
    // tentando conectar e poluindo o log. Trocar de backend é só mudar Otlp:Endpoint.
    private static void AddOtlpExporter(IOpenTelemetryBuilder telemetry, string? endpoint)
    {
        if (string.IsNullOrWhiteSpace(endpoint))
            return;

        telemetry.UseOtlpExporter(OpenTelemetry.Exporter.OtlpExportProtocol.Grpc, new Uri(endpoint));
    }
}
