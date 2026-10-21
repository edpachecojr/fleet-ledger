using FleetLedger.Core.Models;
using FleetLedger.Core.Requests.Customers;

namespace FleetLedger.Api.Data.Stores.Customers;

public interface ICustomerStore
{
    // Rastreado: o handler altera e chama SaveChangesAsync.
    Task<Customer?> FindAsync(Guid organizationId, Guid customerId, CancellationToken cancellationToken);

    Task<bool> CnpjExistsAsync(Guid organizationId, string cnpj, CancellationToken cancellationToken);

    Task<StorePage<Customer>> ListAsync(ListCustomersRequest request, CancellationToken cancellationToken);

    void Add(Customer customer);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
