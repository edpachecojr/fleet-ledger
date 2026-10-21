using FleetLedger.Api.Tests.Fakes;
using FleetLedger.Core.Models;

namespace FleetLedger.Api.Tests.Fixtures;

public static class CustomerFactory
{
    public static Customer Create(Guid organizationId, string cnpj = "11222333000181", DateTimeOffset? deactivatedAt = null)
        => new()
        {
            OrganizationId = organizationId,
            CorporateName = "Agro Sul Ltda",
            Cnpj = cnpj,
            BillingCycle = EBillingCycle.Monthly,
            DeactivatedAt = deactivatedAt,
            CreatedAt = FakeTimeProvider.ReferenceNow.AddDays(-30),
            UpdatedAt = FakeTimeProvider.ReferenceNow.AddDays(-30)
        };
}
