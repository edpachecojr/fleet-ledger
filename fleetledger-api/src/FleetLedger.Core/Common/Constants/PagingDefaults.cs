namespace FleetLedger.Core.Common.Constants;

/// <summary>
/// Limites de paginação aplicados no servidor. O cliente sugere; o servidor decide.
/// </summary>
public static class PagingDefaults
{
    /// <summary>Página usada quando o cliente não informa nenhuma.</summary>
    public const int PageNumber = 1;

    /// <summary>Tamanho de página usado quando o cliente não informa nenhum.</summary>
    public const int PageSize = 25;

    /// <summary>Teto de itens por página, independente do que o cliente pedir.</summary>
    public const int MaxPageSize = 100;
}
