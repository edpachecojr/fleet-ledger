using FleetLedger.Core.Models;
using FleetLedger.Core.Requests.Drivers;

namespace FleetLedger.Api.Data.Stores.Drivers;

public interface IDriverStore
{
    // Rastreado: o handler altera e chama SaveChangesAsync.
    Task<Driver?> FindAsync(Guid organizationId, Guid driverId, CancellationToken cancellationToken);

    Task<bool> CpfExistsAsync(Guid organizationId, string cpf, CancellationToken cancellationToken);

    // ignoredDriverId: na edição, a CNH do próprio motorista não conta como duplicada.
    Task<bool> CnhNumberExistsAsync(Guid organizationId, string cnhNumber, Guid? ignoredDriverId, CancellationToken cancellationToken);

    Task<StorePage<Driver>> ListAsync(ListDriversRequest request, DateOnly today, CancellationToken cancellationToken);

    void Add(Driver driver);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
