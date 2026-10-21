using FleetLedger.Core.Common.Validation;
using FleetLedger.Core.Models;
using FleetLedger.Core.Requests.Vehicles;

namespace FleetLedger.Core.Validators.Vehicles;

/// <summary>
/// Só os destinos manuais: Disponível ou Em manutenção. Se a transição a partir do status
/// atual é permitida, quem decide é o handler (precisa do veículo).
/// </summary>
/// <example><code>ValidationResult result = new ChangeVehicleStatusValidator().Validate(request);</code></example>
public sealed class ChangeVehicleStatusValidator : Validator<ChangeVehicleStatusRequest>
{
    /// <inheritdoc />
    protected override void Rules(ChangeVehicleStatusRequest request, ValidationBuilder rules)
        => rules.When(
            request.Status is not (EVehicleStatus.Available or EVehicleStatus.InMaintenance),
            nameof(request.Status),
            $"Status '{request.Status}' não pode ser escolhido manualmente. " +
            "Esperado: Available (Disponível) ou InMaintenance (Em manutenção); Em viagem é definido pelo sistema");
}
