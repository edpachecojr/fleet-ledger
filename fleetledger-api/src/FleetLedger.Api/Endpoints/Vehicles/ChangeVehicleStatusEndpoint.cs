using FleetLedger.Api.Common.Api;
using FleetLedger.Core.Handlers;
using FleetLedger.Core.Models;
using FleetLedger.Core.Requests.Vehicles;
using FleetLedger.Core.Responses;
using FleetLedger.Core.Security;

namespace FleetLedger.Api.Endpoints.Vehicles;

public sealed class ChangeVehicleStatusEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPut("/{id:guid}/status", HandleAsync)
            .WithName("Vehicles: ChangeStatus")
            .WithSummary("Coloca o veículo em manutenção ou o devolve para Disponível")
            .WithDescription("Em viagem é definido só pelo sistema; veículo em viagem ou inativo responde 409.")
            .AddEndpointFilter<ValidationFilter<ChangeVehicleStatusRequest>>()
            .Produces<Response<Vehicle?>>(StatusCodes.Status200OK)
            .Produces<Response<Vehicle?>>(StatusCodes.Status404NotFound)
            .Produces<Response<Vehicle?>>(StatusCodes.Status409Conflict)
            .ProducesValidationProblem();

    private static async Task<IResult> HandleAsync(
        Guid id,
        IUserContext userContext,
        IVehicleHandler handler,
        ChangeVehicleStatusRequest request,
        CancellationToken cancellationToken)
    {
        request.Id = id;
        request.OrganizationId = await userContext.GetOrganizationIdAsync(cancellationToken);

        Response<Vehicle?> result = await handler.ChangeStatusAsync(request, cancellationToken);

        return TypedResults.Json(result, statusCode: result.Code);
    }
}
