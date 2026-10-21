using FleetLedger.Api.Common.Api;

namespace FleetLedger.Api.Endpoints.Drivers;

// Sem DELETE: motorista com histórico só é inativado (TASK-007).
public static class DriverEndpointGroup
{
    public static void MapDriverEndpoints(this IEndpointRouteBuilder app)
        => app.MapGroup("v1/drivers")
            .WithTags("Drivers")
            .MapEndpoint<CreateDriverEndpoint>()
            .MapEndpoint<ListDriversEndpoint>()
            .MapEndpoint<GetDriverEndpoint>()
            .MapEndpoint<UpdateDriverEndpoint>()
            .MapEndpoint<DeactivateDriverEndpoint>()
            .MapEndpoint<ReactivateDriverEndpoint>();
}
