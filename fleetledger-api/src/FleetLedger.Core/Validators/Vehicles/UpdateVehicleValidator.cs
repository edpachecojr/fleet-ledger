using FleetLedger.Core.Common.Validation;
using FleetLedger.Core.Requests.Vehicles;

namespace FleetLedger.Core.Validators.Vehicles;

/// <summary>Forma da edição: só os campos editáveis (modelo, capacidade, taxa).</summary>
/// <example><code>ValidationResult result = new UpdateVehicleValidator().Validate(request);</code></example>
public sealed class UpdateVehicleValidator : Validator<UpdateVehicleRequest>
{
    /// <inheritdoc />
    protected override void Rules(UpdateVehicleRequest request, ValidationBuilder rules)
        => rules.VehicleSpecification(request.Model, request.CapacityKg, request.MaintenanceProvisionRatePerKm);
}
