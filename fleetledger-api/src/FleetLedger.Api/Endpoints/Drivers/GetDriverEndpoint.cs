using FleetLedger.Api.Common.Api;
using FleetLedger.Core.Handlers;
using FleetLedger.Core.Models;
using FleetLedger.Core.Requests;
using FleetLedger.Core.Responses;
using FleetLedger.Core.Security;

namespace FleetLedger.Api.Endpoints.Drivers;

public sealed class GetDriverEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/{id:guid}", HandleAsync)
            .WithName("Drivers: Get")
            .WithSummary("Detalha um motorista, com a situação documental de hoje")
            .Produces<Response<Driver?>>(StatusCodes.Status200OK)
            .Produces<Response<Driver?>>(StatusCodes.Status404NotFound);

    private static async Task<IResult> HandleAsync(
        Guid id,
        IUserContext userContext,
        IDriverHandler handler,
        CancellationToken cancellationToken)
    {
        ByIdRequest request = new() { Id = id, OrganizationId = await userContext.GetOrganizationIdAsync(cancellationToken) };

        Response<Driver?> result = await handler.GetAsync(request, cancellationToken);

        return TypedResults.Json(result, statusCode: result.Code);
    }
}
