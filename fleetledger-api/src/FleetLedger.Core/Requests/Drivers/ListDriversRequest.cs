using FleetLedger.Core.Models;

namespace FleetLedger.Core.Requests.Drivers;

/// <summary>Listagem de motoristas com busca por nome ou CPF e filtro de situação documental.</summary>
/// <example><code>new ListDriversRequest { DocumentStatus = EDocumentStatus.Expired }</code></example>
public sealed class ListDriversRequest : PagedRequest
{
    /// <summary>Trecho do nome ou do CPF (com ou sem máscara).</summary>
    public string? Search { get; set; }

    /// <summary>Só ativos (true), só inativos (false) ou todos (nulo).</summary>
    public bool? IsActive { get; set; }

    /// <summary>Só motoristas nesta situação documental na data de hoje; nulo para todas.</summary>
    public EDocumentStatus? DocumentStatus { get; set; }
}
