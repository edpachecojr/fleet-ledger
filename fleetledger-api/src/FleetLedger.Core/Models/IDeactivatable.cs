namespace FleetLedger.Core.Models;

/// <summary>
/// Cadastro com histórico que não é excluído, só inativado (veículo, cliente, motorista).
/// Inativo some das seleções de nova viagem; pode ser reativado.
/// </summary>
/// <example><code>public sealed class Customer : BaseModel, IOrganizationOwned, IDeactivatable</code></example>
public interface IDeactivatable
{
    /// <summary>Momento (UTC) da inativação; nulo enquanto ativo.</summary>
    DateTimeOffset? DeactivatedAt { get; set; }

    /// <summary>Verdadeiro enquanto <see cref="DeactivatedAt"/> é nulo.</summary>
    bool IsActive { get; }
}
