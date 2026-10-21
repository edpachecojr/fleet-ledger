using FleetLedger.Core.Models;
using FleetLedger.Core.Requests.Customers;
using FleetLedger.Core.Validators.Documents;
using Microsoft.EntityFrameworkCore;

namespace FleetLedger.Api.Data.Stores.Customers;

// Consultas públicas para os testes conferirem o SQL sem banco (docs/adr/0004).
public sealed class CustomerStore(AppDbContext context) : ICustomerStore
{
    public async Task<Customer?> FindAsync(Guid organizationId, Guid customerId, CancellationToken cancellationToken)
        => await ByIdQuery(organizationId, customerId).SingleOrDefaultAsync(cancellationToken);

    public async Task<bool> CnpjExistsAsync(Guid organizationId, string cnpj, CancellationToken cancellationToken)
        => await context.Customers.ForOrganization(organizationId).AnyAsync(customer => customer.Cnpj == cnpj, cancellationToken);

    public async Task<StorePage<Customer>> ListAsync(ListCustomersRequest request, CancellationToken cancellationToken)
        => await ListQuery(request).ToStorePageAsync(request, cancellationToken);

    public void Add(Customer customer) => context.Customers.Add(customer);

    public async Task SaveChangesAsync(CancellationToken cancellationToken) => await context.SaveChangesAsync(cancellationToken);

    public IQueryable<Customer> ByIdQuery(Guid organizationId, Guid customerId)
        => context.Customers.ForOrganization(organizationId).Where(customer => customer.Id == customerId);

    public IQueryable<Customer> ListQuery(ListCustomersRequest request)
    {
        IQueryable<Customer> query = context.Customers.ForOrganization(request.OrganizationId).AsNoTracking();
        query = WithSearch(query, request.Search);

        if (request.IsActive is bool isActive)
            query = query.Where(customer => (customer.DeactivatedAt == null) == isActive);

        return query.OrderBy(customer => customer.CorporateName);
    }

    // CNPJ guardado só com dígitos: "11.222.333" acha "11222333000181". Termo sem dígito
    // não compara CNPJ, senão o vazio casaria com todos.
    private static IQueryable<Customer> WithSearch(IQueryable<Customer> query, string? search)
    {
        if (string.IsNullOrWhiteSpace(search))
            return query;

        string namePattern = SearchPattern.Contains(search.Trim());
        string cnpjDigits = DigitsOnly.Of(search);
        bool searchesCnpj = cnpjDigits.Length > 0;
        return query.Where(customer => EF.Functions.ILike(customer.CorporateName, namePattern, SearchPattern.EscapeCharacter)
            || (customer.TradeName != null && EF.Functions.ILike(customer.TradeName, namePattern, SearchPattern.EscapeCharacter))
            || (searchesCnpj && customer.Cnpj.Contains(cnpjDigits)));
    }
}
