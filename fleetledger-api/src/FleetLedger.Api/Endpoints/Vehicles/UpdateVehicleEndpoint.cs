using FleetLedger.Api.Common.Api;
using FleetLedger.Core.Handlers;
using FleetLedger.Core.Models;
using FleetLedger.Core.Requests.Vehicles;
using FleetLedger.Core.Responses;
using FleetLedger.Core.Security;

namespace FleetLedger.Api.Endpoints.Vehicles;

public sealed class UpdateVehicleEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPut("/{id:guid}", HandleAsync)
            .WithName("Vehicles: Update")
            .WithSummary("Altera modelo, capacidade e taxa de provisão de um veículo")
            .WithDescription("Placa e hodômetro não são editáveis.")
            .AddEndpointFilter<ValidationFilter<UpdateVehicleRequest>>()
            .Produces<Response<Vehicle?>>(StatusCodes.Status200OK)
            .Produces<Response<Vehicle?>>(StatusCodes.Status404NotFound)
            .ProducesValidationProblem();

    private static async Task<IResult> HandleAsync(
        Guid id,
        IUserContext userContext,
        IVehicleHandler handler,
        UpdateVehicleRequest request,
        CancellationToken cancellationToken)
    {
        request.Id = id;
        request.OrganizationId = await userContext.GetOrganizationIdAsync(cancellationToken);

        Response<Vehicle?> result = await handler.UpdateAsync(request, cancellationToken);

        return TypedResults.Json(result, statusCode: result.Code);
    }
}
