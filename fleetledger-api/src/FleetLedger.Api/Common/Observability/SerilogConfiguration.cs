using Serilog;
using Serilog.Formatting;
using Serilog.Formatting.Compact;
using Serilog.Templates;

namespace FleetLedger.Api.Common.Observability;

public static class SerilogConfiguration
{
    private const string DevelopmentTemplate =
        "[{@t:HH:mm:ss} {@l:u3}] {@m} {Rest(true)}\n{@x}";

    public static void AddStructuredLogging(this WebApplicationBuilder builder)
        => builder.Services.AddSerilog((services, logger) =>
        {
            logger
                .ReadFrom.Configuration(builder.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentName()
                .Enrich.WithProperty("service.name", ObservabilityDefaults.ServiceName)
                .WriteTo.Console(ConsoleFormatter(builder.Environment));

            AddOtlpSink(logger, builder);
        });

    // Console legível em desenvolvimento; JSON compacto fora dele, porque log de
    // produção é dado consultável, não texto para leitura humana.
    private static ITextFormatter ConsoleFormatter(IHostEnvironment environment)
        => environment.IsDevelopment()
            ? new ExpressionTemplate(DevelopmentTemplate)
            : new CompactJsonFormatter();

    private static void AddOtlpSink(LoggerConfiguration logger, WebApplicationBuilder builder)
    {
        string? endpoint = builder.Configuration[ObservabilityDefaults.OtlpEndpointKey];
        if (string.IsNullOrWhiteSpace(endpoint))
            return;

        logger.WriteTo.OpenTelemetry(options =>
        {
            options.Endpoint = endpoint;
            options.ResourceAttributes = new Dictionary<string, object>
            {
                ["service.name"] = ObservabilityDefaults.ServiceName,
                ["service.version"] = ObservabilityDefaults.ServiceVersion,
                ["deployment.environment"] = builder.Environment.EnvironmentName
            };
        });
    }
}
