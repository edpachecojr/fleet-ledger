namespace FleetLedger.Core.Models;

/// <summary>
/// Onde uma conta do Plano de Contas pode ser escolhida num lançamento manual. Conta sem
/// contexto (<see cref="LedgerAccount.EntryContext"/> nulo) é sistêmica: só o sistema lança nela.
/// </summary>
/// <example><code>accounts.Where(account => account.EntryContext == ELedgerEntryContext.Trip)</code></example>
public enum ELedgerEntryContext
{
    /// <summary>Despesas lançadas numa viagem (combustível, pedágio…).</summary>
    Trip = 1,

    /// <summary>Despesas fixas mensais da frota (seguro, aluguel…).</summary>
    FixedExpense = 2,

    /// <summary>Registro de manutenção executada (oficina, peças).</summary>
    Maintenance = 3,

    /// <summary>Fatura ao cliente: frete, descontos e acréscimos.</summary>
    Billing = 4
}
