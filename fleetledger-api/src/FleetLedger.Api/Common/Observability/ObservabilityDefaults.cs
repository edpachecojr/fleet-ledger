using System.Diagnostics;

namespace FleetLedger.Api.Common.Observability;

public static class ObservabilityDefaults
{
    public const string ServiceName = "fleetledger-api";
    public const string ServiceVersion = "0.1.0";
    public const string OtlpEndpointKey = "Otlp:Endpoint";

    public static readonly ActivitySource ActivitySource = new(ServiceName, ServiceVersion);
}
