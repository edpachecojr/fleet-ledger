using FleetLedger.Api.Common.Api;
using FleetLedger.Core.Handlers;
using FleetLedger.Core.Models;
using FleetLedger.Core.Requests;
using FleetLedger.Core.Responses;
using FleetLedger.Core.Security;

namespace FleetLedger.Api.Endpoints.Customers;

public sealed class DeactivateCustomerEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/{id:guid}/deactivate", HandleAsync)
            .WithName("Customers: Deactivate")
            .WithSummary("Inativa o cliente; ele some da abertura de viagem")
            .Produces<Response<Customer?>>(StatusCodes.Status200OK)
            .Produces<Response<Customer?>>(StatusCodes.Status404NotFound);

    private static async Task<IResult> HandleAsync(
        Guid id,
        IUserContext userContext,
        ICustomerHandler handler,
        CancellationToken cancellationToken)
    {
        ByIdRequest request = new() { Id = id, OrganizationId = await userContext.GetOrganizationIdAsync(cancellationToken) };

        Response<Customer?> result = await handler.DeactivateAsync(request, cancellationToken);

        return TypedResults.Json(result, statusCode: result.Code);
    }
}
