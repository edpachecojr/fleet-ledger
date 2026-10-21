using FleetLedger.Api.Common.Api;
using FleetLedger.Core.Common.Constants;
using FleetLedger.Core.Handlers;
using FleetLedger.Core.Models;
using FleetLedger.Core.Requests.Vehicles;
using FleetLedger.Core.Responses;
using FleetLedger.Core.Security;

namespace FleetLedger.Api.Endpoints.Vehicles;

public sealed class ListVehiclesEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/", HandleAsync)
            .WithName("Vehicles: List")
            .WithSummary("Lista veículos com busca por placa ou modelo")
            .WithDescription("allocatable=true devolve só os elegíveis para nova viagem (ativos e disponíveis).")
            .Produces<PagedResponse<List<Vehicle>>>(StatusCodes.Status200OK);

    // Parâmetros explícitos, e não [AsParameters]: o OrganizationId do request não pode
    // virar parâmetro de query no contrato (docs/adr/0003).
    private static async Task<IResult> HandleAsync(
        IUserContext userContext,
        IVehicleHandler handler,
        CancellationToken cancellationToken,
        string? search = null,
        bool? isActive = null,
        bool allocatable = false,
        int pageNumber = PagingDefaults.PageNumber,
        int pageSize = PagingDefaults.PageSize)
    {
        ListVehiclesRequest request = new()
        {
            Search = search,
            IsActive = isActive,
            Allocatable = allocatable,
            PageNumber = pageNumber,
            PageSize = pageSize,
            OrganizationId = await userContext.GetOrganizationIdAsync(cancellationToken)
        };

        PagedResponse<List<Vehicle>> result = await handler.ListAsync(request, cancellationToken);

        return TypedResults.Json(result, statusCode: result.Code);
    }
}
