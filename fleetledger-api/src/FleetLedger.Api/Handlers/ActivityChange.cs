using FleetLedger.Core.Models;

namespace FleetLedger.Api.Handlers;

// Inativar e reativar são idempotentes em todo cadastro: pedir o estado que o registro
// já tem não grava nada (e não troca a data da inativação original).
public static class ActivityChange
{
    public static bool TryApply<TEntity>(TEntity entity, bool deactivate, DateTimeOffset now)
        where TEntity : BaseModel, IDeactivatable
    {
        if (entity.IsActive != deactivate)
            return false;

        entity.DeactivatedAt = deactivate ? now : null;
        entity.UpdatedAt = now;
        return true;
    }
}
