using FleetLedger.Api.Common.Api;
using FleetLedger.Core.Handlers;
using FleetLedger.Core.Models;
using FleetLedger.Core.Requests.Vehicles;
using FleetLedger.Core.Responses;
using FleetLedger.Core.Security;

namespace FleetLedger.Api.Endpoints.Vehicles;

public sealed class CreateVehicleEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/", HandleAsync)
            .WithName("Vehicles: Create")
            .WithSummary("Cadastra um veículo, com status inicial Disponível")
            .AddEndpointFilter<ValidationFilter<CreateVehicleRequest>>()
            .Produces<Response<Vehicle?>>(StatusCodes.Status201Created)
            .Produces<Response<Vehicle?>>(StatusCodes.Status409Conflict)
            .ProducesValidationProblem();

    private static async Task<IResult> HandleAsync(
        IUserContext userContext,
        IVehicleHandler handler,
        CreateVehicleRequest request,
        CancellationToken cancellationToken)
    {
        request.OrganizationId = await userContext.GetOrganizationIdAsync(cancellationToken);

        Response<Vehicle?> result = await handler.CreateAsync(request, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Created($"/v1/vehicles/{result.Data?.Id}", result)
            : TypedResults.Json(result, statusCode: result.Code);
    }
}
