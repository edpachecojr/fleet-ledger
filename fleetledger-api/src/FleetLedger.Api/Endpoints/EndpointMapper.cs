using FleetLedger.Api.Common.Api;
using FleetLedger.Api.Common.Health;

namespace FleetLedger.Api.Endpoints;

// Permanece curto: cada módulo registra o próprio <Modulo>EndpointGroup.
public static class EndpointMapper
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapHealthEndpoint();
        app.MapHttpContract();
    }
}
