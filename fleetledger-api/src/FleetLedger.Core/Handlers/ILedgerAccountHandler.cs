using FleetLedger.Core.Models;
using FleetLedger.Core.Responses;

namespace FleetLedger.Core.Handlers;

/// <summary>Consulta do Plano de Contas, que é somente leitura.</summary>
public interface ILedgerAccountHandler
{
    /// <summary>Todos os grupos da DRE com suas contas, na ordem dos códigos.</summary>
    /// <example><code>Response&lt;List&lt;LedgerAccountGroup&gt;&gt; chart = await handler.ListChartAsync(cancellationToken);</code></example>
    Task<Response<List<LedgerAccountGroup>>> ListChartAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Contas que aceitam lançamento manual no <paramref name="entryContext"/>, para listas de seleção.
    /// Contas sistêmicas nunca aparecem.
    /// </summary>
    /// <example><code>await handler.ListSelectableAsync(ELedgerEntryContext.Trip, cancellationToken);</code></example>
    Task<Response<List<LedgerAccount>>> ListSelectableAsync(
        ELedgerEntryContext entryContext,
        CancellationToken cancellationToken);
}
