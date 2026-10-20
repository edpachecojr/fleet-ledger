using FleetLedger.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FleetLedger.Api.Data;

public static class OrganizationQueryExtension
{
    // Filtro explícito, sem query filter global: quem lê o código vê o isolamento.
    // Toda consulta de entidade IOrganizationOwned começa por aqui.
    public static IQueryable<TEntity> ForOrganization<TEntity>(this DbSet<TEntity> entities, Guid organizationId)
        where TEntity : class, IOrganizationOwned
    {
        if (organizationId == Guid.Empty)
            throw new ArgumentException(
                $"OrganizationId vazio ao consultar {typeof(TEntity).Name}. " +
                "Esperado: id resolvido por IUserContext.GetOrganizationIdAsync.",
                nameof(organizationId));

        return entities.Where(entity => entity.OrganizationId == organizationId);
    }
}
