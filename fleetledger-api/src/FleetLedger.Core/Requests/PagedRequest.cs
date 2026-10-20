using FleetLedger.Core.Common.Constants;

namespace FleetLedger.Core.Requests;

/// <summary>
/// Requisição de listagem paginada. O limite em <see cref="PagingDefaults.MaxPageSize"/>
/// é aplicado no servidor, nunca confiado ao cliente.
/// </summary>
public abstract class PagedRequest : Request
{
    /// <summary>Página solicitada, a partir de 1.</summary>
    public int PageNumber { get; set; } = PagingDefaults.PageNumber;

    /// <summary>Itens por página, até <see cref="PagingDefaults.MaxPageSize"/>.</summary>
    public int PageSize { get; set; } = PagingDefaults.PageSize;
}
