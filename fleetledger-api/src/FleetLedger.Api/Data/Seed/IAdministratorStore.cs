using FleetLedger.Api.Models;
using FleetLedger.Core.Models;

namespace FleetLedger.Api.Data.Seed;

public interface IAdministratorStore
{
    Task<bool> HasAnyUserAsync(CancellationToken cancellationToken);

    Task CreateAsync(Organization organization, User administrator, string password);
}
