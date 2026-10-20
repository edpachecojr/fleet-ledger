using FleetLedger.Core.Common.Validation;

namespace FleetLedger.Api.Tests.Fakes;

public sealed class FakeValidator<TRequest>(ValidationResult result) : IValidator<TRequest>
{
    public ValidationResult Validate(TRequest request) => result;
}
