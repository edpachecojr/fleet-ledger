using FleetLedger.Api.Common.Api;
using FleetLedger.Core.Common.Constants;
using FleetLedger.Core.Handlers;
using FleetLedger.Core.Models;
using FleetLedger.Core.Requests.Drivers;
using FleetLedger.Core.Responses;
using FleetLedger.Core.Security;

namespace FleetLedger.Api.Endpoints.Drivers;

public sealed class ListDriversEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/", HandleAsync)
            .WithName("Drivers: List")
            .WithSummary("Lista motoristas com busca por nome ou CPF e filtro de situação documental")
            .WithDescription("isActive=true devolve só os selecionáveis em nova viagem.")
            .Produces<PagedResponse<List<Driver>>>(StatusCodes.Status200OK);

    // Parâmetros explícitos, e não [AsParameters]: o OrganizationId do request não pode
    // virar parâmetro de query no contrato (docs/adr/0003).
    private static async Task<IResult> HandleAsync(
        IUserContext userContext,
        IDriverHandler handler,
        CancellationToken cancellationToken,
        string? search = null,
        bool? isActive = null,
        EDocumentStatus? documentStatus = null,
        int pageNumber = PagingDefaults.PageNumber,
        int pageSize = PagingDefaults.PageSize)
    {
        ListDriversRequest request = new()
        {
            Search = search,
            IsActive = isActive,
            DocumentStatus = documentStatus,
            PageNumber = pageNumber,
            PageSize = pageSize,
            OrganizationId = await userContext.GetOrganizationIdAsync(cancellationToken)
        };

        PagedResponse<List<Driver>> result = await handler.ListAsync(request, cancellationToken);

        return TypedResults.Json(result, statusCode: result.Code);
    }
}
