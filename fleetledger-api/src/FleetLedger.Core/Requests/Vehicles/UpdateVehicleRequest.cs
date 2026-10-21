using System.Text.Json.Serialization;

namespace FleetLedger.Core.Requests.Vehicles;

/// <summary>
/// Edição do cadastro. Placa e hodômetros não são editáveis: a placa é a identidade do
/// veículo e o hodômetro atual só muda pelo sistema.
/// </summary>
/// <example><code>new UpdateVehicleRequest { Model = "Volvo FH 540", CapacityKg = 27000, MaintenanceProvisionRatePerKm = 0.15m }</code></example>
public sealed class UpdateVehicleRequest : Request
{
    /// <summary>Id do veículo, lido da rota.</summary>
    [JsonIgnore]
    public Guid Id { get; set; }

    /// <summary>Marca e modelo.</summary>
    public string Model { get; set; } = string.Empty;

    /// <summary>Capacidade de carga, em kg (maior que zero).</summary>
    public int CapacityKg { get; set; }

    /// <summary>Provisão de manutenção em R$/km, com até 2 casas; vale para viagens fechadas depois.</summary>
    public decimal MaintenanceProvisionRatePerKm { get; set; }
}
