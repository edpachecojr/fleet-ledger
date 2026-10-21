using FleetLedger.Api.Data.Stores;
using FleetLedger.Api.Data.Stores.Vehicles;
using FleetLedger.Core.Models;
using FleetLedger.Core.Requests.Vehicles;

namespace FleetLedger.Api.Tests.Fakes;

// Guarda veículos de várias organizações e filtra como o store real: é o que permite
// provar no handler que uma organização não enxerga a outra.
public sealed class FakeVehicleStore : IVehicleStore
{
    private readonly List<Vehicle> _vehicles = [];

    public int SaveCount { get; private set; }
    public IReadOnlyList<Vehicle> Vehicles => _vehicles;
    public ListVehiclesRequest? LastListRequest { get; private set; }

    public FakeVehicleStore With(Vehicle vehicle)
    {
        _vehicles.Add(vehicle);
        return this;
    }

    public Task<Vehicle?> FindAsync(Guid organizationId, Guid vehicleId, CancellationToken cancellationToken)
        => Task.FromResult(_vehicles.SingleOrDefault(v => v.OrganizationId == organizationId && v.Id == vehicleId));

    public Task<bool> PlateExistsAsync(Guid organizationId, string plate, CancellationToken cancellationToken)
        => Task.FromResult(_vehicles.Any(v => v.OrganizationId == organizationId && v.Plate == plate));

    public Task<StorePage<Vehicle>> ListAsync(ListVehiclesRequest request, CancellationToken cancellationToken)
    {
        LastListRequest = request;
        List<Vehicle> owned = _vehicles.Where(v => v.OrganizationId == request.OrganizationId).ToList();
        return Task.FromResult(new StorePage<Vehicle>(owned, owned.Count));
    }

    public void Add(Vehicle vehicle) => _vehicles.Add(vehicle);

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        SaveCount++;
        return Task.CompletedTask;
    }
}
