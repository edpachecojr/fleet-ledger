namespace FleetLedger.Core.Requests.Vehicles;

/// <summary>Cadastro de veículo. O hodômetro atual nasce igual ao inicial.</summary>
/// <example><code>new CreateVehicleRequest { Plate = "ABC1D23", Model = "Volvo FH 540", CapacityKg = 27000 }</code></example>
public sealed class CreateVehicleRequest : Request
{
    /// <summary>Placa no formato antigo ou Mercosul; hífen e minúsculas são aceitos.</summary>
    public string Plate { get; set; } = string.Empty;

    /// <summary>Marca e modelo.</summary>
    public string Model { get; set; } = string.Empty;

    /// <summary>Capacidade de carga, em kg (maior que zero).</summary>
    public int CapacityKg { get; set; }

    /// <summary>Hodômetro no cadastro, em km (zero ou mais).</summary>
    public int InitialOdometerKm { get; set; }

    /// <summary>Provisão de manutenção em R$/km, com até 2 casas (zero ou mais).</summary>
    public decimal MaintenanceProvisionRatePerKm { get; set; }
}
