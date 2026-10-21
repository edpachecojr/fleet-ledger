using FleetLedger.Api.Tests.Fakes;
using FleetLedger.Core.Models;

namespace FleetLedger.Api.Tests.Fixtures;

public static class DriverFactory
{
    public static Driver Create(
        Guid organizationId,
        string cpf = "52998224725",
        string cnhNumber = "12345678901",
        DateOnly? cnhExpiresOn = null,
        DateOnly? toxicologyExamExpiresOn = null)
        => new()
        {
            OrganizationId = organizationId,
            Name = "João da Silva",
            Cpf = cpf,
            Phone = "11987654321",
            CnhNumber = cnhNumber,
            CnhCategory = ECnhCategory.E,
            CnhExpiresOn = cnhExpiresOn ?? FakeBusinessCalendar.ReferenceToday.AddYears(2),
            ToxicologyExamExpiresOn = toxicologyExamExpiresOn,
            DefaultCommissionPercent = 12.5m,
            CreatedAt = FakeTimeProvider.ReferenceNow.AddDays(-30),
            UpdatedAt = FakeTimeProvider.ReferenceNow.AddDays(-30)
        };
}
