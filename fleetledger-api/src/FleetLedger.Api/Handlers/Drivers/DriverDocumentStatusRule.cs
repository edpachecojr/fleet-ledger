using System.Linq.Expressions;
using FleetLedger.Core.Models;

namespace FleetLedger.Api.Handlers.Drivers;

// Situação documental (TASK-007, decisão de 2026-10-21): Vencido se alguma validade é
// anterior a hoje; Vencendo se a mais próxima cai de hoje até hoje + 30 dias (inclusive);
// senão Regular. Toxicológico em branco não conta. As expressões filtram a listagem no SQL
// com a mesma regra que Evaluate aplica em memória.
public static class DriverDocumentStatusRule
{
    public const int ExpiringWindowDays = 30;

    public static EDocumentStatus Evaluate(Driver driver, DateOnly today)
    {
        DateOnly nearest = NearestExpiration(driver);
        if (nearest < today)
            return EDocumentStatus.Expired;

        return nearest <= today.AddDays(ExpiringWindowDays) ? EDocumentStatus.ExpiringSoon : EDocumentStatus.Regular;
    }

    public static Expression<Func<Driver, bool>> HasStatus(EDocumentStatus status, DateOnly today)
    {
        DateOnly limit = today.AddDays(ExpiringWindowDays);
        return status switch
        {
            EDocumentStatus.Expired => driver => driver.CnhExpiresOn < today
                || (driver.ToxicologyExamExpiresOn != null && driver.ToxicologyExamExpiresOn < today),
            EDocumentStatus.ExpiringSoon => driver => driver.CnhExpiresOn >= today
                && (driver.ToxicologyExamExpiresOn == null || driver.ToxicologyExamExpiresOn >= today)
                && (driver.CnhExpiresOn <= limit || (driver.ToxicologyExamExpiresOn != null && driver.ToxicologyExamExpiresOn <= limit)),
            EDocumentStatus.Regular => driver => driver.CnhExpiresOn > limit
                && (driver.ToxicologyExamExpiresOn == null || driver.ToxicologyExamExpiresOn > limit),
            _ => throw new ArgumentOutOfRangeException(nameof(status), status,
                $"Situação documental '{status}' desconhecida. Esperado: Regular, ExpiringSoon ou Expired.")
        };
    }

    private static DateOnly NearestExpiration(Driver driver)
        => driver.ToxicologyExamExpiresOn is DateOnly toxicology && toxicology < driver.CnhExpiresOn
            ? toxicology
            : driver.CnhExpiresOn;
}
