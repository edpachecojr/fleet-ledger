using FleetLedger.Api.Common.Api;
using FleetLedger.Core.Handlers;
using FleetLedger.Core.Models;
using FleetLedger.Core.Requests;
using FleetLedger.Core.Responses;
using FleetLedger.Core.Security;

namespace FleetLedger.Api.Endpoints.Vehicles;

public sealed class DeactivateVehicleEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/{id:guid}/deactivate", HandleAsync)
            .WithName("Vehicles: Deactivate")
            .WithSummary("Inativa o veículo; ele sai das seleções e do rateio")
            .Produces<Response<Vehicle?>>(StatusCodes.Status200OK)
            .Produces<Response<Vehicle?>>(StatusCodes.Status404NotFound);

    private static async Task<IResult> HandleAsync(
        Guid id,
        IUserContext userContext,
        IVehicleHandler handler,
        CancellationToken cancellationToken)
    {
        ByIdRequest request = new() { Id = id, OrganizationId = await userContext.GetOrganizationIdAsync(cancellationToken) };

        Response<Vehicle?> result = await handler.DeactivateAsync(request, cancellationToken);

        return TypedResults.Json(result, statusCode: result.Code);
    }
}
