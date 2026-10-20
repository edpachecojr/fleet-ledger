namespace FleetLedger.Api.Data;

public interface IUserOrganizationReader
{
    Task<Guid?> FindOrganizationIdAsync(Guid userId, CancellationToken cancellationToken);
}
