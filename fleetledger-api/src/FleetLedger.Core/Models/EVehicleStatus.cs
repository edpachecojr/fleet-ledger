namespace FleetLedger.Core.Models;

/// <summary>
/// Status operacional do veículo. <see cref="OnTrip"/> só é definido pelo sistema
/// (abertura, fechamento e cancelamento de viagem), nunca manualmente.
/// </summary>
public enum EVehicleStatus
{
    /// <summary>Pronto para ser alocado numa viagem. Status inicial do cadastro.</summary>
    Available = 1,

    /// <summary>Alocado numa viagem aberta.</summary>
    OnTrip = 2,

    /// <summary>Parado para manutenção; não pode ser alocado.</summary>
    InMaintenance = 3
}
