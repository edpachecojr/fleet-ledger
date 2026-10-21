namespace FleetLedger.Api.Data.Stores;

// Uma página de resultados e o total sem paginação, para montar o PagedResponse.
public sealed record StorePage<TEntity>(List<TEntity> Items, int TotalCount);
