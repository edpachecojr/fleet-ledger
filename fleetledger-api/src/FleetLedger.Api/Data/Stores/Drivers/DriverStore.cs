using FleetLedger.Api.Handlers.Drivers;
using FleetLedger.Core.Models;
using FleetLedger.Core.Requests.Drivers;
using FleetLedger.Core.Validators.Documents;
using Microsoft.EntityFrameworkCore;

namespace FleetLedger.Api.Data.Stores.Drivers;

// Consultas públicas para os testes conferirem o SQL sem banco (docs/adr/0004).
public sealed class DriverStore(AppDbContext context) : IDriverStore
{
    public async Task<Driver?> FindAsync(Guid organizationId, Guid driverId, CancellationToken cancellationToken)
        => await ByIdQuery(organizationId, driverId).SingleOrDefaultAsync(cancellationToken);

    public async Task<bool> CpfExistsAsync(Guid organizationId, string cpf, CancellationToken cancellationToken)
        => await context.Drivers.ForOrganization(organizationId).AnyAsync(driver => driver.Cpf == cpf, cancellationToken);

    public async Task<bool> CnhNumberExistsAsync(
        Guid organizationId,
        string cnhNumber,
        Guid? ignoredDriverId,
        CancellationToken cancellationToken)
        => await context.Drivers.ForOrganization(organizationId)
            .AnyAsync(driver => driver.CnhNumber == cnhNumber && driver.Id != ignoredDriverId, cancellationToken);

    public async Task<StorePage<Driver>> ListAsync(ListDriversRequest request, DateOnly today, CancellationToken cancellationToken)
        => await ListQuery(request, today).ToStorePageAsync(request, cancellationToken);

    public void Add(Driver driver) => context.Drivers.Add(driver);

    public async Task SaveChangesAsync(CancellationToken cancellationToken) => await context.SaveChangesAsync(cancellationToken);

    public IQueryable<Driver> ByIdQuery(Guid organizationId, Guid driverId)
        => context.Drivers.ForOrganization(organizationId).Where(driver => driver.Id == driverId);

    public IQueryable<Driver> ListQuery(ListDriversRequest request, DateOnly today)
    {
        IQueryable<Driver> query = context.Drivers.ForOrganization(request.OrganizationId).AsNoTracking();
        query = WithSearch(query, request.Search);

        if (request.IsActive is bool isActive)
            query = query.Where(driver => (driver.DeactivatedAt == null) == isActive);
        if (request.DocumentStatus is EDocumentStatus status)
            query = query.Where(DriverDocumentStatusRule.HasStatus(status, today));

        return query.OrderBy(driver => driver.Name);
    }

    // CPF guardado só com dígitos; termo sem dígito não compara CPF (o vazio casaria com todos).
    private static IQueryable<Driver> WithSearch(IQueryable<Driver> query, string? search)
    {
        if (string.IsNullOrWhiteSpace(search))
            return query;

        string namePattern = SearchPattern.Contains(search.Trim());
        string cpfDigits = DigitsOnly.Of(search);
        bool searchesCpf = cpfDigits.Length > 0;
        return query.Where(driver => EF.Functions.ILike(driver.Name, namePattern, SearchPattern.EscapeCharacter)
            || (searchesCpf && driver.Cpf.Contains(cpfDigits)));
    }
}
