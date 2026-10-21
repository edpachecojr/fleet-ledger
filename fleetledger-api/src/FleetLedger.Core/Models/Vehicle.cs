namespace FleetLedger.Core.Models;

/// <summary>
/// Veículo da frota. O hodômetro atual começa igual ao inicial e depois só o sistema o
/// altera (fechamento de viagem, registro de manutenção), nunca a edição do cadastro.
/// </summary>
/// <example><code>new Vehicle { Plate = "ABC1D23", Model = "Volvo FH 540", CapacityKg = 27000 }</code></example>
public sealed class Vehicle : BaseModel, IOrganizationOwned
{
    /// <inheritdoc />
    public Guid OrganizationId { get; set; }

    /// <summary>Placa normalizada (maiúsculas, sem hífen): "ABC1234" ou "ABC1D23". Imutável.</summary>
    public string Plate { get; init; } = string.Empty;

    /// <summary>Marca e modelo, como o gestor identifica o veículo.</summary>
    public string Model { get; set; } = string.Empty;

    /// <summary>Capacidade de carga, em quilogramas.</summary>
    public int CapacityKg { get; set; }

    /// <summary>Hodômetro no cadastro, em km. Não muda depois.</summary>
    public int InitialOdometerKm { get; init; }

    /// <summary>Hodômetro atual, em km. Atualizado só pelo sistema.</summary>
    public int CurrentOdometerKm { get; set; }

    /// <summary>
    /// Provisão de manutenção em R$ por km rodado (2 casas). Alterar vale só para viagens
    /// fechadas depois da alteração.
    /// </summary>
    public decimal MaintenanceProvisionRatePerKm { get; set; }

    /// <summary>Status operacional; começa em <see cref="EVehicleStatus.Available"/>.</summary>
    public EVehicleStatus Status { get; set; } = EVehicleStatus.Available;

    /// <summary>Momento (UTC) da inativação; nulo enquanto o veículo está ativo.</summary>
    public DateTimeOffset? DeactivatedAt { get; set; }

    /// <summary>Veículo ativo: aparece nas seleções e entra no rateio de custo fixo.</summary>
    public bool IsActive => DeactivatedAt is null;
}
