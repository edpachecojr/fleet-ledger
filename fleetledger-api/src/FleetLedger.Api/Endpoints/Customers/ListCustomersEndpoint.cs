using FleetLedger.Api.Common.Api;
using FleetLedger.Core.Common.Constants;
using FleetLedger.Core.Handlers;
using FleetLedger.Core.Models;
using FleetLedger.Core.Requests.Customers;
using FleetLedger.Core.Responses;
using FleetLedger.Core.Security;

namespace FleetLedger.Api.Endpoints.Customers;

public sealed class ListCustomersEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/", HandleAsync)
            .WithName("Customers: List")
            .WithSummary("Lista clientes com busca por razão social, nome fantasia ou CNPJ")
            .WithDescription("isActive=true devolve só os selecionáveis em nova viagem.")
            .Produces<PagedResponse<List<Customer>>>(StatusCodes.Status200OK);

    // Parâmetros explícitos, e não [AsParameters]: o OrganizationId do request não pode
    // virar parâmetro de query no contrato (docs/adr/0003).
    private static async Task<IResult> HandleAsync(
        IUserContext userContext,
        ICustomerHandler handler,
        CancellationToken cancellationToken,
        string? search = null,
        bool? isActive = null,
        int pageNumber = PagingDefaults.PageNumber,
        int pageSize = PagingDefaults.PageSize)
    {
        ListCustomersRequest request = new()
        {
            Search = search,
            IsActive = isActive,
            PageNumber = pageNumber,
            PageSize = pageSize,
            OrganizationId = await userContext.GetOrganizationIdAsync(cancellationToken)
        };

        PagedResponse<List<Customer>> result = await handler.ListAsync(request, cancellationToken);

        return TypedResults.Json(result, statusCode: result.Code);
    }
}
