using FleetLedger.Core.Common.Validation;
using FleetLedger.Core.Requests.Drivers;

namespace FleetLedger.Core.Validators.Drivers;

/// <summary>Forma da edição: os mesmos campos do cadastro, menos o CPF.</summary>
/// <example><code>ValidationResult result = new UpdateDriverValidator().Validate(request);</code></example>
public sealed class UpdateDriverValidator : Validator<UpdateDriverRequest>
{
    /// <inheritdoc />
    protected override void Rules(UpdateDriverRequest request, ValidationBuilder rules) => rules.DriverProfile(request);
}
