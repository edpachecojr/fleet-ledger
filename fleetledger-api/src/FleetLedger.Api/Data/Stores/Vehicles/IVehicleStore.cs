using FleetLedger.Core.Models;
using FleetLedger.Core.Requests.Vehicles;

namespace FleetLedger.Api.Data.Stores.Vehicles;

public interface IVehicleStore
{
    // Rastreado: o handler altera e chama SaveChangesAsync.
    Task<Vehicle?> FindAsync(Guid organizationId, Guid vehicleId, CancellationToken cancellationToken);

    Task<bool> PlateExistsAsync(Guid organizationId, string plate, CancellationToken cancellationToken);

    Task<StorePage<Vehicle>> ListAsync(ListVehiclesRequest request, CancellationToken cancellationToken);

    void Add(Vehicle vehicle);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
