using FleetLedger.Api.Common.Api;
using FleetLedger.Core.Handlers;
using FleetLedger.Core.Models;
using FleetLedger.Core.Requests.Drivers;
using FleetLedger.Core.Responses;
using FleetLedger.Core.Security;

namespace FleetLedger.Api.Endpoints.Drivers;

public sealed class UpdateDriverEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPut("/{id:guid}", HandleAsync)
            .WithName("Drivers: Update")
            .WithSummary("Altera os dados de um motorista")
            .WithDescription("O CPF não é editável; a comissão nova não afeta viagens já abertas.")
            .AddEndpointFilter<ValidationFilter<UpdateDriverRequest>>()
            .Produces<Response<Driver?>>(StatusCodes.Status200OK)
            .Produces<Response<Driver?>>(StatusCodes.Status404NotFound)
            .Produces<Response<Driver?>>(StatusCodes.Status409Conflict)
            .ProducesValidationProblem();

    private static async Task<IResult> HandleAsync(
        Guid id,
        IUserContext userContext,
        IDriverHandler handler,
        UpdateDriverRequest request,
        CancellationToken cancellationToken)
    {
        request.Id = id;
        request.OrganizationId = await userContext.GetOrganizationIdAsync(cancellationToken);

        Response<Driver?> result = await handler.UpdateAsync(request, cancellationToken);

        return TypedResults.Json(result, statusCode: result.Code);
    }
}
