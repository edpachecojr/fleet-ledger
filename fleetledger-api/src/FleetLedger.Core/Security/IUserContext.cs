namespace FleetLedger.Core.Security;

/// <summary>
/// Identidade do usuário da requisição atual. É a única fonte de <c>OrganizationId</c>:
/// a organização nunca vem de header, query string ou body do cliente.
/// </summary>
/// <example>
/// <code>
/// Guid organizationId = await userContext.GetOrganizationIdAsync(cancellationToken);
/// IQueryable&lt;Vehicle&gt; vehicles = context.Vehicles.ForOrganization(organizationId);
/// </code>
/// </example>
public interface IUserContext
{
    /// <summary>
    /// Usuário autenticado, lido das claims do access token. Lança quando a claim falta ou
    /// é inválida: um id vazio removeria o filtro da consulta.
    /// </summary>
    Guid UserId { get; }

    /// <summary>
    /// Organização do usuário, buscada no banco a partir de <see cref="UserId"/> e mantida
    /// em cache por 5 minutos. Lança quando o usuário não existe.
    /// </summary>
    Task<Guid> GetOrganizationIdAsync(CancellationToken cancellationToken);
}
