namespace FleetLedger.Core.Requests.Vehicles;

/// <summary>Listagem de veículos com busca e filtros, paginada.</summary>
/// <example><code>new ListVehiclesRequest { Search = "volvo", IsActive = true }</code></example>
public sealed class ListVehiclesRequest : PagedRequest
{
    /// <summary>Trecho da placa (com ou sem hífen) ou do modelo, sem diferenciar maiúsculas.</summary>
    public string? Search { get; set; }

    /// <summary>Só ativos (true), só inativos (false) ou todos (nulo).</summary>
    public bool? IsActive { get; set; }

    /// <summary>Só os que podem ser alocados numa nova viagem (ativos e disponíveis).</summary>
    public bool Allocatable { get; set; }
}
