using FleetLedger.Api.Common.Api;

namespace FleetLedger.Api.Endpoints.Vehicles;

// Sem DELETE: veículo com histórico só é inativado (TASK-004).
public static class VehicleEndpointGroup
{
    public static void MapVehicleEndpoints(this IEndpointRouteBuilder app)
        => app.MapGroup("v1/vehicles")
            .WithTags("Vehicles")
            .MapEndpoint<CreateVehicleEndpoint>()
            .MapEndpoint<ListVehiclesEndpoint>()
            .MapEndpoint<GetVehicleEndpoint>()
            .MapEndpoint<UpdateVehicleEndpoint>()
            .MapEndpoint<ChangeVehicleStatusEndpoint>()
            .MapEndpoint<DeactivateVehicleEndpoint>()
            .MapEndpoint<ReactivateVehicleEndpoint>();
}
