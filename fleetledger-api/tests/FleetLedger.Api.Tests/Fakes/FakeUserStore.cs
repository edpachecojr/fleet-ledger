using FleetLedger.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace FleetLedger.Api.Tests.Fakes;

// Store em memória para exercitar o UserManager real (hash, normalização, bloqueio) sem banco.
public sealed class FakeUserStore : IUserPasswordStore<User>, IUserEmailStore<User>, IUserLockoutStore<User>
{
    private readonly Dictionary<Guid, User> _users = [];

    public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        => Task.FromResult(user.Id.ToString());

    public Task<string?> GetUserNameAsync(User user, CancellationToken cancellationToken)
        => Task.FromResult(user.UserName);

    public Task SetUserNameAsync(User user, string? userName, CancellationToken cancellationToken)
        => Apply(() => user.UserName = userName);

    public Task<string?> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        => Task.FromResult(user.NormalizedUserName);

    public Task SetNormalizedUserNameAsync(User user, string? normalizedName, CancellationToken cancellationToken)
        => Apply(() => user.NormalizedUserName = normalizedName);

    public Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
    {
        _users[user.Id] = user;
        return Task.FromResult(IdentityResult.Success);
    }

    public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        => Task.FromResult(IdentityResult.Success);

    public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
    {
        _users.Remove(user.Id);
        return Task.FromResult(IdentityResult.Success);
    }

    public Task<User?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        => Task.FromResult(_users.Values.FirstOrDefault(user => user.Id.ToString() == userId));

    public Task<User?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        => Task.FromResult(_users.Values.FirstOrDefault(user => user.NormalizedUserName == normalizedUserName));

    public Task SetPasswordHashAsync(User user, string? passwordHash, CancellationToken cancellationToken)
        => Apply(() => user.PasswordHash = passwordHash);

    public Task<string?> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        => Task.FromResult(user.PasswordHash);

    public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        => Task.FromResult(user.PasswordHash is not null);

    public Task SetEmailAsync(User user, string? email, CancellationToken cancellationToken)
        => Apply(() => user.Email = email);

    public Task<string?> GetEmailAsync(User user, CancellationToken cancellationToken)
        => Task.FromResult(user.Email);

    public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
        => Task.FromResult(user.EmailConfirmed);

    public Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        => Apply(() => user.EmailConfirmed = confirmed);

    public Task<User?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        => Task.FromResult(_users.Values.FirstOrDefault(user => user.NormalizedEmail == normalizedEmail));

    public Task<string?> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
        => Task.FromResult(user.NormalizedEmail);

    public Task SetNormalizedEmailAsync(User user, string? normalizedEmail, CancellationToken cancellationToken)
        => Apply(() => user.NormalizedEmail = normalizedEmail);

    public Task<DateTimeOffset?> GetLockoutEndDateAsync(User user, CancellationToken cancellationToken)
        => Task.FromResult(user.LockoutEnd);

    public Task SetLockoutEndDateAsync(User user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        => Apply(() => user.LockoutEnd = lockoutEnd);

    public Task<int> IncrementAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        => Task.FromResult(++user.AccessFailedCount);

    public Task ResetAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        => Apply(() => user.AccessFailedCount = 0);

    public Task<int> GetAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        => Task.FromResult(user.AccessFailedCount);

    public Task<bool> GetLockoutEnabledAsync(User user, CancellationToken cancellationToken)
        => Task.FromResult(user.LockoutEnabled);

    public Task SetLockoutEnabledAsync(User user, bool enabled, CancellationToken cancellationToken)
        => Apply(() => user.LockoutEnabled = enabled);

    public void Dispose()
    {
    }

    private static Task Apply(Action change)
    {
        change();
        return Task.CompletedTask;
    }
}
