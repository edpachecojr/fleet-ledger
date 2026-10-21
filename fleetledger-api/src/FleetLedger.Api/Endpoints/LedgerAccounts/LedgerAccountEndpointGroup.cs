using FleetLedger.Api.Common.Api;

namespace FleetLedger.Api.Endpoints.LedgerAccounts;

// Só leitura de propósito: o Plano de Contas não pode ser alterado (TASK-003).
public static class LedgerAccountEndpointGroup
{
    public static void MapLedgerAccountEndpoints(this IEndpointRouteBuilder app)
        => app.MapGroup("v1/ledger-accounts")
            .WithTags("LedgerAccounts")
            .MapEndpoint<ListLedgerAccountGroupsEndpoint>()
            .MapEndpoint<ListSelectableLedgerAccountsEndpoint>();
}
