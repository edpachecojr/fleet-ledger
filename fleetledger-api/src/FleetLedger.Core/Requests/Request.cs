using System.Text.Json.Serialization;

namespace FleetLedger.Core.Requests;

/// <summary>
/// Base de toda requisição autenticada. O <see cref="OrganizationId"/> é atribuído pelo
/// endpoint a partir de <c>IUserContext</c> e ignorado na serialização, para que o
/// cliente não consiga enviá-lo nem o veja no contrato OpenAPI.
/// </summary>
/// <example>
/// <code>request.OrganizationId = await userContext.GetOrganizationIdAsync(cancellationToken);</code>
/// </example>
public abstract class Request
{
    /// <summary>Organização do usuário autenticado, resolvida no servidor.</summary>
    [JsonIgnore]
    public Guid OrganizationId { get; set; }
}
