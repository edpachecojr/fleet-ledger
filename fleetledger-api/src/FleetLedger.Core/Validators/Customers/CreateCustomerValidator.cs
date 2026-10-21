using FleetLedger.Core.Common.Validation;
using FleetLedger.Core.Requests.Customers;
using FleetLedger.Core.Validators.Documents;

namespace FleetLedger.Core.Validators.Customers;

/// <summary>Forma do cadastro; CNPJ duplicado é conferido pelo handler.</summary>
/// <example><code>ValidationResult result = new CreateCustomerValidator().Validate(request);</code></example>
public sealed class CreateCustomerValidator : Validator<CreateCustomerRequest>
{
    /// <inheritdoc />
    protected override void Rules(CreateCustomerRequest request, ValidationBuilder rules)
        => rules
            .Required(nameof(request.Cnpj), request.Cnpj, "CNPJ é obrigatório")
            .When(!string.IsNullOrWhiteSpace(request.Cnpj) && !CnpjNumber.IsValid(DigitsOnly.Of(request.Cnpj)),
                nameof(request.Cnpj),
                $"CNPJ '{request.Cnpj}' inválido. Esperado: 14 dígitos com verificadores corretos (ex.: 11.222.333/0001-81)")
            .CustomerProfileFields(new(request.CorporateName, request.TradeName, request.Phone, request.Email,
                request.BillingCycle, request.PaymentTermDays));
}
