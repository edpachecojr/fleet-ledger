using FleetLedger.Api.Common.Api;
using FleetLedger.Api.Endpoints.Auth;
using FleetLedger.Api.Endpoints.Health;
using FleetLedger.Api.Endpoints.LedgerAccounts;
using FleetLedger.Api.Endpoints.Vehicles;

namespace FleetLedger.Api.Endpoints;

// Permanece curto: cada módulo registra o próprio <Modulo>EndpointGroup.
public static class EndpointMapper
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapEndpoint<GetHealthEndpoint>();
        app.MapAuthEndpoints();
        app.MapLedgerAccountEndpoints();
        app.MapVehicleEndpoints();
        app.MapHttpContract();
    }
}
