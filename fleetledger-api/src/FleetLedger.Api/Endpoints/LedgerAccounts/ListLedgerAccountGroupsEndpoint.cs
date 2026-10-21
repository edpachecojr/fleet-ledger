using FleetLedger.Api.Common.Api;
using FleetLedger.Core.Handlers;
using FleetLedger.Core.Models;
using FleetLedger.Core.Responses;

namespace FleetLedger.Api.Endpoints.LedgerAccounts;

public sealed class ListLedgerAccountGroupsEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/", HandleAsync)
            .WithName("LedgerAccounts: ListChart")
            .WithSummary("Lista o Plano de Contas: grupos da DRE com suas contas")
            .Produces<Response<List<LedgerAccountGroup>>>(StatusCodes.Status200OK);

    private static async Task<IResult> HandleAsync(ILedgerAccountHandler handler, CancellationToken cancellationToken)
    {
        Response<List<LedgerAccountGroup>> result = await handler.ListChartAsync(cancellationToken);

        return TypedResults.Json(result, statusCode: result.Code);
    }
}
