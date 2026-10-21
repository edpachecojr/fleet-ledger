using FleetLedger.Api.Data.Stores;
using FleetLedger.Api.Data.Stores.Drivers;
using FleetLedger.Core.Models;
using FleetLedger.Core.Requests.Drivers;

namespace FleetLedger.Api.Tests.Fakes;

// Guarda motoristas de várias organizações e filtra como o store real, para provar o isolamento.
public sealed class FakeDriverStore : IDriverStore
{
    private readonly List<Driver> _drivers = [];

    public int SaveCount { get; private set; }
    public IReadOnlyList<Driver> Drivers => _drivers;
    public DateOnly? LastListToday { get; private set; }

    public FakeDriverStore With(Driver driver)
    {
        _drivers.Add(driver);
        return this;
    }

    public Task<Driver?> FindAsync(Guid organizationId, Guid driverId, CancellationToken cancellationToken)
        => Task.FromResult(_drivers.SingleOrDefault(d => d.OrganizationId == organizationId && d.Id == driverId));

    public Task<bool> CpfExistsAsync(Guid organizationId, string cpf, CancellationToken cancellationToken)
        => Task.FromResult(_drivers.Any(d => d.OrganizationId == organizationId && d.Cpf == cpf));

    public Task<bool> CnhNumberExistsAsync(Guid organizationId, string cnhNumber, Guid? ignoredDriverId, CancellationToken cancellationToken)
        => Task.FromResult(_drivers.Any(d => d.OrganizationId == organizationId && d.CnhNumber == cnhNumber && d.Id != ignoredDriverId));

    public Task<StorePage<Driver>> ListAsync(ListDriversRequest request, DateOnly today, CancellationToken cancellationToken)
    {
        LastListToday = today;
        List<Driver> owned = _drivers.Where(d => d.OrganizationId == request.OrganizationId).ToList();
        return Task.FromResult(new StorePage<Driver>(owned, owned.Count));
    }

    public void Add(Driver driver) => _drivers.Add(driver);

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        SaveCount++;
        return Task.CompletedTask;
    }
}
