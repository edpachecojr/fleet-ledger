using FleetLedger.Core.Common.Constants;

namespace FleetLedger.Core.Responses;

/// <summary>
/// Resposta de listagem com os metadados de página.
/// </summary>
/// <example><code>return new PagedResponse&lt;List&lt;Vehicle&gt;&gt;(vehicles, totalCount, request.PageNumber, request.PageSize);</code></example>
/// <typeparam name="TData">Coleção devolvida.</typeparam>
public class PagedResponse<TData> : Response<TData>
{
    /// <summary>Construtor usado pela desserialização.</summary>
    public PagedResponse()
    {
    }

    /// <summary>Cria uma página de sucesso com o total de itens disponíveis.</summary>
    public PagedResponse(
        TData? data,
        int totalCount,
        int currentPage = PagingDefaults.PageNumber,
        int pageSize = PagingDefaults.PageSize)
        : base(data)
    {
        TotalCount = totalCount;
        CurrentPage = currentPage;
        PageSize = pageSize;
    }

    /// <summary>Página devolvida, a partir de 1.</summary>
    public int CurrentPage { get; set; }

    /// <summary>Itens por página usados na consulta.</summary>
    public int PageSize { get; set; } = PagingDefaults.PageSize;

    /// <summary>Total de itens disponíveis em todas as páginas.</summary>
    public int TotalCount { get; set; }

    /// <summary>Total de páginas; zero quando <see cref="PageSize"/> não é positivo.</summary>
    public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
}
