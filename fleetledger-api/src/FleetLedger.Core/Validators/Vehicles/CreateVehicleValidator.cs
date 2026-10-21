using FleetLedger.Core.Common.Validation;
using FleetLedger.Core.Requests.Vehicles;

namespace FleetLedger.Core.Validators.Vehicles;

/// <summary>Forma do cadastro; placa duplicada é conferida pelo handler, que conhece o banco.</summary>
/// <example><code>ValidationResult result = new CreateVehicleValidator().Validate(request);</code></example>
public sealed class CreateVehicleValidator : Validator<CreateVehicleRequest>
{
    /// <inheritdoc />
    protected override void Rules(CreateVehicleRequest request, ValidationBuilder rules)
        => rules
            .Required(nameof(request.Plate), request.Plate, "Placa é obrigatória")
            .When(!string.IsNullOrWhiteSpace(request.Plate) && !VehiclePlate.IsValid(VehiclePlate.Normalize(request.Plate)),
                nameof(request.Plate),
                $"Placa '{request.Plate}' inválida. Esperado: formato antigo (ABC-1234) ou Mercosul (ABC1D23)")
            .When(request.InitialOdometerKm < 0, nameof(request.InitialOdometerKm),
                $"Hodômetro inicial não pode ser negativo (recebido: {request.InitialOdometerKm} km)")
            .VehicleSpecification(request.Model, request.CapacityKg, request.MaintenanceProvisionRatePerKm);
}
