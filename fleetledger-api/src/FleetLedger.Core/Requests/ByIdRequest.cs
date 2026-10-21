using System.Text.Json.Serialization;

namespace FleetLedger.Core.Requests;

/// <summary>
/// Operação sobre um registro identificado pela rota (consultar, inativar, reativar).
/// O <see cref="Id"/> vem da URL, nunca do corpo.
/// </summary>
/// <example><code>new ByIdRequest { Id = vehicleId, OrganizationId = organizationId }</code></example>
public sealed class ByIdRequest : Request
{
    /// <summary>Id do registro, lido da rota.</summary>
    [JsonIgnore]
    public Guid Id { get; set; }
}
