using FleetLedger.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace FleetLedger.Api.Models;

// Fica em Api, e não em Core, porque herda de IdentityUser: Core não conhece infraestrutura.
public sealed class User : IdentityUser<Guid>, IOrganizationOwned
{
    public User()
    {
        Id = Guid.CreateVersion7();
    }

    public Guid OrganizationId { get; set; }
}
