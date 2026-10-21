namespace FleetLedger.Core.Requests.Customers;

/// <summary>Listagem de clientes com busca por razão social, nome fantasia ou CNPJ, paginada.</summary>
/// <example><code>new ListCustomersRequest { Search = "agro", IsActive = true }</code></example>
public sealed class ListCustomersRequest : PagedRequest
{
    /// <summary>Trecho da razão social, do nome fantasia ou do CNPJ (com ou sem máscara).</summary>
    public string? Search { get; set; }

    /// <summary>Só ativos (true), só inativos (false) ou todos (nulo).</summary>
    public bool? IsActive { get; set; }
}
