using FleetLedger.Core.Common.Validation;

namespace FleetLedger.Api.Common.Api;

public static class ValidatorRegistrationExtension
{
    // Varre o assembly de Core; validador não tem estado, então é singleton.
    public static void AddValidators(this WebApplicationBuilder builder)
    {
        foreach ((Type contract, Type implementation) in FindValidators())
            builder.Services.AddSingleton(contract, implementation);
    }

    private static IEnumerable<(Type Contract, Type Implementation)> FindValidators()
        => typeof(IValidator<>).Assembly.GetTypes()
            .Where(type => type is { IsAbstract: false, IsInterface: false })
            .SelectMany(
                type => type.GetInterfaces().Where(IsValidatorContract),
                (type, contract) => (contract, type));

    private static bool IsValidatorContract(Type contract)
        => contract.IsGenericType && contract.GetGenericTypeDefinition() == typeof(IValidator<>);
}
