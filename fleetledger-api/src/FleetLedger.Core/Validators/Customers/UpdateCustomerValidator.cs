using FleetLedger.Core.Common.Validation;
using FleetLedger.Core.Requests.Customers;

namespace FleetLedger.Core.Validators.Customers;

/// <summary>Forma da edição: os mesmos campos do cadastro, menos o CNPJ.</summary>
/// <example><code>ValidationResult result = new UpdateCustomerValidator().Validate(request);</code></example>
public sealed class UpdateCustomerValidator : Validator<UpdateCustomerRequest>
{
    /// <inheritdoc />
    protected override void Rules(UpdateCustomerRequest request, ValidationBuilder rules)
        => rules.CustomerProfileFields(new(request.CorporateName, request.TradeName, request.Phone, request.Email,
            request.BillingCycle, request.PaymentTermDays));
}
