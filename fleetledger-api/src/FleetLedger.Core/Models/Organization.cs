namespace FleetLedger.Core.Models;

/// <summary>
/// A empresa que usa o sistema. Isola logicamente os dados: tudo que pertence a ela
/// implementa <see cref="IOrganizationOwned"/>.
/// </summary>
public sealed class Organization : BaseModel
{
    /// <summary>Nome de exibição da organização.</summary>
    public string Name { get; set; } = string.Empty;
}
