using FleetLedger.Api.Common.Api;
using FleetLedger.Core.Handlers;
using FleetLedger.Core.Models;
using FleetLedger.Core.Responses;

namespace FleetLedger.Api.Endpoints.LedgerAccounts;

public sealed class ListSelectableLedgerAccountsEndpoint : IEndpoint
{
    // entryContext obrigatório: sem contexto, a lista misturaria contas de usos diferentes.
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/selectable", HandleAsync)
            .WithName("LedgerAccounts: ListSelectable")
            .WithSummary("Lista as contas que aceitam lançamento manual num contexto")
            .WithDescription("Contas sistêmicas (provisão, adiantamento, comissão) nunca aparecem.")
            .Produces<Response<List<LedgerAccount>>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);

    private static async Task<IResult> HandleAsync(
        ILedgerAccountHandler handler,
        ELedgerEntryContext entryContext,
        CancellationToken cancellationToken)
    {
        Response<List<LedgerAccount>> result = await handler.ListSelectableAsync(entryContext, cancellationToken);

        return TypedResults.Json(result, statusCode: result.Code);
    }
}
