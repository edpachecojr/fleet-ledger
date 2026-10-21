using FleetLedger.Core.Models;

namespace FleetLedger.Api.Handlers.Vehicles;

// Máquina de estados da troca MANUAL (TASK-005): Disponível ↔ Em manutenção. Em viagem
// entra e sai só pela viagem (TASK-011/013/017), e veículo inativo precisa ser reativado
// antes (decisão de 2026-10-21). O destino já vem restrito pelo ChangeVehicleStatusValidator.
public static class VehicleStatusTransition
{
    public static EVehicleStatusChangeVerdict Evaluate(Vehicle vehicle, EVehicleStatus target)
    {
        if (!vehicle.IsActive)
            return EVehicleStatusChangeVerdict.BlockedInactive;
        if (vehicle.Status == EVehicleStatus.OnTrip)
            return EVehicleStatusChangeVerdict.BlockedOnTrip;

        return vehicle.Status == target ? EVehicleStatusChangeVerdict.Unchanged : EVehicleStatusChangeVerdict.Allowed;
    }
}
