using FleetLedger.Api.Common.Security;
using FleetLedger.Api.Models;
using FleetLedger.Api.Tests.Fakes;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using OptionsFactory = Microsoft.Extensions.Options.Options;

namespace FleetLedger.Api.Tests.Fixtures;

// UserManager real, com a política da API, sobre o FakeUserStore.
public static class UserManagerFactory
{
    // Uma iteração de PBKDF2: o algoritmo é o mesmo, só fica rápido o bastante para teste.
    private const int FastHashIterations = 1;

    public static UserManager<User> Create(FakeUserStore store)
    {
        IdentityOptions identityOptions = new();
        AdministratorIdentityPolicy.Apply(identityOptions);

        return new UserManager<User>(
            store,
            OptionsFactory.Create(identityOptions),
            new PasswordHasher<User>(OptionsFactory.Create(new PasswordHasherOptions { IterationCount = FastHashIterations })),
            [new UserValidator<User>()],
            [new PasswordValidator<User>()],
            new UpperInvariantLookupNormalizer(),
            new IdentityErrorDescriber(),
            new ServiceCollection().BuildServiceProvider(),
            NullLogger<UserManager<User>>.Instance);
    }
}
