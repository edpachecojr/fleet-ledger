using FleetLedger.Core.Requests;
using Microsoft.EntityFrameworkCore;

namespace FleetLedger.Api.Data.Stores;

public static class QueryPagingExtension
{
    // Usa os valores efetivos do PagedRequest: o cliente sugere, o servidor limita.
    public static async Task<StorePage<TEntity>> ToStorePageAsync<TEntity>(
        this IQueryable<TEntity> query,
        PagedRequest request,
        CancellationToken cancellationToken)
    {
        int totalCount = await query.CountAsync(cancellationToken);
        List<TEntity> items = await query
            .Skip((request.EffectivePageNumber - 1) * request.EffectivePageSize)
            .Take(request.EffectivePageSize)
            .ToListAsync(cancellationToken);

        return new StorePage<TEntity>(items, totalCount);
    }
}
