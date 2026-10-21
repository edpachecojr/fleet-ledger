using System.Text.Json.Serialization;
using FleetLedger.Core.Models;

namespace FleetLedger.Core.Requests.Vehicles;

/// <summary>
/// Troca manual do status operacional: só entre Disponível e Em manutenção. "Em viagem" é
/// definido apenas pelo sistema (TASK-005).
/// </summary>
/// <example><code>new ChangeVehicleStatusRequest { Status = EVehicleStatus.InMaintenance }</code></example>
public sealed class ChangeVehicleStatusRequest : Request
{
    /// <summary>Id do veículo, lido da rota.</summary>
    [JsonIgnore]
    public Guid Id { get; set; }

    /// <summary>Novo status: <see cref="EVehicleStatus.Available"/> ou <see cref="EVehicleStatus.InMaintenance"/>.</summary>
    public EVehicleStatus Status { get; set; }
}
