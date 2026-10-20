using System.Diagnostics;
using System.Security.Claims;
using Serilog;

namespace FleetLedger.Api.Common.Observability;

public static class ObservabilityExtension
{
    private const string RequestLogTemplate =
        "HTTP {RequestMethod} {RequestPath} respondeu {StatusCode} em {Elapsed:0.0000} ms";

    public static void AddObservability(this WebApplicationBuilder builder)
    {
        builder.AddStructuredLogging();
        builder.AddTelemetry();
    }

    public static void UseObservability(this WebApplication app)
        => app.UseSerilogRequestLogging(options =>
        {
            options.MessageTemplate = RequestLogTemplate;
            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                diagnosticContext.Set("UserId", httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anonymous");
                diagnosticContext.Set("TraceId", Activity.Current?.TraceId.ToString());
            };
        });
}
