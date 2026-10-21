namespace FleetLedger.Api.Handlers.Vehicles;

// Resultado da avaliação de VehicleStatusTransition: só Allowed grava.
public enum EVehicleStatusChangeVerdict
{
    Allowed = 1,
    Unchanged = 2,
    BlockedOnTrip = 3,
    BlockedInactive = 4
}
