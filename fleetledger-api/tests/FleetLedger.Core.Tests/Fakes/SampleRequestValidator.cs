using FleetLedger.Core.Common.Validation;

namespace FleetLedger.Core.Tests.Fakes;

public sealed class SampleRequestValidator : Validator<SampleRequest>
{
    protected override void Rules(SampleRequest request, ValidationBuilder rules)
        => rules.Required(nameof(request.Name), request.Name, "Nome é obrigatório");
}
