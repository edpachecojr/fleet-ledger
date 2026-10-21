using FleetLedger.Api.Common.Api;
using FleetLedger.Core.Handlers;
using FleetLedger.Core.Models;
using FleetLedger.Core.Requests.Drivers;
using FleetLedger.Core.Responses;
using FleetLedger.Core.Security;

namespace FleetLedger.Api.Endpoints.Drivers;

public sealed class CreateDriverEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/", HandleAsync)
            .WithName("Drivers: Create")
            .WithSummary("Cadastra um motorista")
            .AddEndpointFilter<ValidationFilter<CreateDriverRequest>>()
            .Produces<Response<Driver?>>(StatusCodes.Status201Created)
            .Produces<Response<Driver?>>(StatusCodes.Status409Conflict)
            .ProducesValidationProblem();

    private static async Task<IResult> HandleAsync(
        IUserContext userContext,
        IDriverHandler handler,
        CreateDriverRequest request,
        CancellationToken cancellationToken)
    {
        request.OrganizationId = await userContext.GetOrganizationIdAsync(cancellationToken);

        Response<Driver?> result = await handler.CreateAsync(request, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Created($"/v1/drivers/{result.Data?.Id}", result)
            : TypedResults.Json(result, statusCode: result.Code);
    }
}
