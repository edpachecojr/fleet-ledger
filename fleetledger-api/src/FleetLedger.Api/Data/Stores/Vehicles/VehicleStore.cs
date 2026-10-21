using FleetLedger.Api.Handlers.Vehicles;
using FleetLedger.Core.Models;
using FleetLedger.Core.Requests.Vehicles;
using FleetLedger.Core.Validators.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace FleetLedger.Api.Data.Stores.Vehicles;

// Consultas públicas para os testes conferirem o SQL sem banco (docs/adr/0004).
public sealed class VehicleStore(AppDbContext context) : IVehicleStore
{
    public async Task<Vehicle?> FindAsync(Guid organizationId, Guid vehicleId, CancellationToken cancellationToken)
        => await ByIdQuery(organizationId, vehicleId).SingleOrDefaultAsync(cancellationToken);

    public async Task<bool> PlateExistsAsync(Guid organizationId, string plate, CancellationToken cancellationToken)
        => await context.Vehicles.ForOrganization(organizationId).AnyAsync(vehicle => vehicle.Plate == plate, cancellationToken);

    public async Task<StorePage<Vehicle>> ListAsync(ListVehiclesRequest request, CancellationToken cancellationToken)
        => await ListQuery(request).ToStorePageAsync(request, cancellationToken);

    public void Add(Vehicle vehicle) => context.Vehicles.Add(vehicle);

    public async Task SaveChangesAsync(CancellationToken cancellationToken) => await context.SaveChangesAsync(cancellationToken);

    public IQueryable<Vehicle> ByIdQuery(Guid organizationId, Guid vehicleId)
        => context.Vehicles.ForOrganization(organizationId).Where(vehicle => vehicle.Id == vehicleId);

    public IQueryable<Vehicle> ListQuery(ListVehiclesRequest request)
    {
        IQueryable<Vehicle> query = context.Vehicles.ForOrganization(request.OrganizationId).AsNoTracking();
        query = WithSearch(query, request.Search);

        if (request.Status is EVehicleStatus status)
            query = query.Where(vehicle => vehicle.Status == status);
        if (request.IsActive is bool isActive)
            query = query.Where(vehicle => (vehicle.DeactivatedAt == null) == isActive);
        if (request.Allocatable)
            query = query.Where(VehicleAllocationRule.IsAllocatableExpression);

        return query.OrderBy(vehicle => vehicle.Plate);
    }

    // A placa é guardada normalizada, então o termo também é ("abc-1d" acha "ABC1D23").
    private static IQueryable<Vehicle> WithSearch(IQueryable<Vehicle> query, string? search)
    {
        if (string.IsNullOrWhiteSpace(search))
            return query;

        string plateTerm = VehiclePlate.Normalize(search);
        string modelPattern = SearchPattern.Contains(search.Trim());
        return query.Where(vehicle => vehicle.Plate.Contains(plateTerm)
            || EF.Functions.ILike(vehicle.Model, modelPattern, SearchPattern.EscapeCharacter));
    }
}
