using FleetLedger.Core.Common.Validation;

namespace FleetLedger.Core.Validators.Vehicles;

/// <summary>Regras dos campos editáveis do veículo, comuns ao cadastro e à edição.</summary>
/// <example><code>rules.VehicleSpecification(request.Model, request.CapacityKg, request.MaintenanceProvisionRatePerKm);</code></example>
public static class VehicleSpecificationRules
{
    /// <summary>Mesmo limite da coluna <c>model</c>.</summary>
    public const int ModelMaxLength = 80;

    /// <summary>Casas decimais da taxa de provisão (R$ com centavos).</summary>
    public const int RateDecimalPlaces = 2;

    /// <summary>Modelo obrigatório, capacidade positiva e taxa não negativa com até 2 casas.</summary>
    public static ValidationBuilder VehicleSpecification(
        this ValidationBuilder rules,
        string? model,
        int capacityKg,
        decimal maintenanceProvisionRatePerKm)
        => rules
            .Required("Model", model, "Modelo é obrigatório")
            .MaxLength("Model", model, ModelMaxLength, $"Modelo deve ter até {ModelMaxLength} caracteres")
            .When(capacityKg <= 0, "CapacityKg", $"Capacidade deve ser maior que zero (recebido: {capacityKg} kg)")
            .When(maintenanceProvisionRatePerKm < 0, "MaintenanceProvisionRatePerKm",
                $"Taxa de provisão não pode ser negativa (recebido: {maintenanceProvisionRatePerKm})")
            .When(decimal.Round(maintenanceProvisionRatePerKm, RateDecimalPlaces) != maintenanceProvisionRatePerKm,
                "MaintenanceProvisionRatePerKm",
                $"Taxa de provisão deve ter até {RateDecimalPlaces} casas decimais (recebido: {maintenanceProvisionRatePerKm})");
}
