namespace FleetLedger.Core.Models;

/// <summary>
/// Conta analítica do Plano de Contas, onde os lançamentos são classificados.
/// Fixa: gravada pela migration, sem criação, edição ou exclusão pelo administrador.
/// </summary>
/// <example><code>new LedgerAccount { Code = "3.01", Name = "Combustível", EntryContext = ELedgerEntryContext.Trip }</code></example>
public sealed class LedgerAccount : BaseModel
{
    /// <summary>Código hierárquico ("3.01"): o prefixo é o código do grupo.</summary>
    public string Code { get; init; } = string.Empty;

    /// <summary>Nome de exibição da conta.</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>Grupo da DRE a que a conta pertence.</summary>
    public Guid GroupId { get; init; }

    /// <summary>
    /// Contexto em que a conta aceita lançamento manual; nulo quando é sistêmica
    /// (ex.: Provisão de manutenção, Adiantamento a motorista).
    /// </summary>
    public ELedgerEntryContext? EntryContext { get; init; }
}
