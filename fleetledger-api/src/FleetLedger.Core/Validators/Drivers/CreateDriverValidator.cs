using FleetLedger.Core.Common.Validation;
using FleetLedger.Core.Requests.Drivers;
using FleetLedger.Core.Validators.Documents;

namespace FleetLedger.Core.Validators.Drivers;

/// <summary>Forma do cadastro; CPF e CNH duplicados são conferidos pelo handler.</summary>
/// <example><code>ValidationResult result = new CreateDriverValidator().Validate(request);</code></example>
public sealed class CreateDriverValidator : Validator<CreateDriverRequest>
{
    /// <inheritdoc />
    protected override void Rules(CreateDriverRequest request, ValidationBuilder rules)
        => rules
            .Required(nameof(request.Cpf), request.Cpf, "CPF é obrigatório")
            .When(!string.IsNullOrWhiteSpace(request.Cpf) && !CpfNumber.IsValid(DigitsOnly.Of(request.Cpf)),
                nameof(request.Cpf),
                $"CPF '{request.Cpf}' inválido. Esperado: 11 dígitos com verificadores corretos (ex.: 529.982.247-25)")
            .DriverProfile(request);
}
