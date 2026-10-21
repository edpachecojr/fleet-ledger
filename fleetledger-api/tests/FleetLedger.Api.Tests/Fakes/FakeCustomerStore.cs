using FleetLedger.Api.Data.Stores;
using FleetLedger.Api.Data.Stores.Customers;
using FleetLedger.Core.Models;
using FleetLedger.Core.Requests.Customers;

namespace FleetLedger.Api.Tests.Fakes;

// Guarda clientes de várias organizações e filtra como o store real, para provar o isolamento.
public sealed class FakeCustomerStore : ICustomerStore
{
    private readonly List<Customer> _customers = [];

    public int SaveCount { get; private set; }
    public IReadOnlyList<Customer> Customers => _customers;

    public FakeCustomerStore With(Customer customer)
    {
        _customers.Add(customer);
        return this;
    }

    public Task<Customer?> FindAsync(Guid organizationId, Guid customerId, CancellationToken cancellationToken)
        => Task.FromResult(_customers.SingleOrDefault(c => c.OrganizationId == organizationId && c.Id == customerId));

    public Task<bool> CnpjExistsAsync(Guid organizationId, string cnpj, CancellationToken cancellationToken)
        => Task.FromResult(_customers.Any(c => c.OrganizationId == organizationId && c.Cnpj == cnpj));

    public Task<StorePage<Customer>> ListAsync(ListCustomersRequest request, CancellationToken cancellationToken)
    {
        List<Customer> owned = _customers.Where(c => c.OrganizationId == request.OrganizationId).ToList();
        return Task.FromResult(new StorePage<Customer>(owned, owned.Count));
    }

    public void Add(Customer customer) => _customers.Add(customer);

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        SaveCount++;
        return Task.CompletedTask;
    }
}
