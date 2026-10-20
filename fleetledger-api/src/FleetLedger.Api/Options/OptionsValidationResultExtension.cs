using FleetLedger.Core.Common.Validation;
using Microsoft.Extensions.Options;

namespace FleetLedger.Api.Options;

public static class OptionsValidationResultExtension
{
    // Reaproveita o ValidationBuilder de Core: configuração e entrada seguem a mesma regra.
    public static ValidateOptionsResult ToOptionsResult(this ValidationResult result)
        => result.IsValid
            ? ValidateOptionsResult.Success
            : ValidateOptionsResult.Fail(result.Errors.Select(error => error.Message));
}
