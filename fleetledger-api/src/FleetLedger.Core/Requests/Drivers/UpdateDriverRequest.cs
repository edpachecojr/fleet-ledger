using System.Text.Json.Serialization;

namespace FleetLedger.Core.Requests.Drivers;

/// <summary>Edição do motorista: tudo menos o CPF. A comissão nova não afeta viagens já abertas.</summary>
/// <example><code>new UpdateDriverRequest { Name = "João da Silva", DefaultCommissionPercent = 12.5m }</code></example>
public sealed class UpdateDriverRequest : DriverProfileFields
{
    /// <summary>Id do motorista, lido da rota.</summary>
    [JsonIgnore]
    public Guid Id { get; set; }
}
