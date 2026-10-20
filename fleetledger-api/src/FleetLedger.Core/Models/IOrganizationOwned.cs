namespace FleetLedger.Core.Models;

/// <summary>
/// Marca uma entidade que pertence a uma <see cref="Organization"/>. Toda leitura dessas
/// entidades passa por <c>ForOrganization(organizationId)</c>, e toda escrita recebe o
/// <see cref="OrganizationId"/> explicitamente no endpoint, a partir de <c>IUserContext</c>.
/// </summary>
/// <example><code>public sealed class Vehicle : BaseModel, IOrganizationOwned</code></example>
public interface IOrganizationOwned
{
    /// <summary>Organização dona do registro. Nunca vem de header, query string ou body.</summary>
    Guid OrganizationId { get; set; }
}
