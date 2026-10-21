using FleetLedger.Api.Common.Api;
using FleetLedger.Core.Handlers;
using FleetLedger.Core.Models;
using FleetLedger.Core.Requests;
using FleetLedger.Core.Responses;
using FleetLedger.Core.Security;

namespace FleetLedger.Api.Endpoints.Drivers;

public sealed class DeactivateDriverEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/{id:guid}/deactivate", HandleAsync)
            .WithName("Drivers: Deactivate")
            .WithSummary("Inativa o motorista; ele some da abertura de viagem")
            .Produces<Response<Driver?>>(StatusCodes.Status200OK)
            .Produces<Response<Driver?>>(StatusCodes.Status404NotFound);

    private static async Task<IResult> HandleAsync(
        Guid id,
        IUserContext userContext,
        IDriverHandler handler,
        CancellationToken cancellationToken)
    {
        ByIdRequest request = new() { Id = id, OrganizationId = await userContext.GetOrganizationIdAsync(cancellationToken) };

        Response<Driver?> result = await handler.DeactivateAsync(request, cancellationToken);

        return TypedResults.Json(result, statusCode: result.Code);
    }
}
