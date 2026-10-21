namespace FleetLedger.Core.Models;

/// <summary>
/// Grupo da DRE do Plano de Contas (ex.: "Custos fixos"). Define em que linha do
/// demonstrativo as contas dele aparecem. Fixo: gravado pela migration, sem edição.
/// </summary>
/// <example><code>new LedgerAccountGroup { Code = "6", Name = "Custos fixos" }</code></example>
public sealed class LedgerAccountGroup : BaseModel
{
    /// <summary>Código hierárquico do grupo ("1", "2"…), na ordem da DRE.</summary>
    public string Code { get; init; } = string.Empty;

    /// <summary>Nome de exibição do grupo.</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>Contas analíticas do grupo, ordenadas pelo código.</summary>
    public List<LedgerAccount> Accounts { get; init; } = [];
}
