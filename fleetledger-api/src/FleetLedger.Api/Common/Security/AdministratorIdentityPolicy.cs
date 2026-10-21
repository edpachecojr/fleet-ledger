using Microsoft.AspNetCore.Identity;

namespace FleetLedger.Api.Common.Security;

// Política do Identity num só lugar: a API e os testes do verificador usam a mesma.
public static class AdministratorIdentityPolicy
{
    // Bloqueio de 15 min após 5 senhas erradas (decisão da TASK-002, docs/PROGRESS.md).
    public const int MaxFailedAccessAttempts = 5;
    public static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(15);

    public static void Apply(IdentityOptions options)
    {
        options.User.RequireUniqueEmail = true;
        options.Lockout.AllowedForNewUsers = true;
        options.Lockout.MaxFailedAccessAttempts = MaxFailedAccessAttempts;
        options.Lockout.DefaultLockoutTimeSpan = LockoutDuration;
    }
}
