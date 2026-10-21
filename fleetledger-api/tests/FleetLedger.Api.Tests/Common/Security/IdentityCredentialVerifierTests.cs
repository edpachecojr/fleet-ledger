using FleetLedger.Api.Common.Security;
using FleetLedger.Api.Models;
using FleetLedger.Api.Tests.Fakes;
using FleetLedger.Api.Tests.Fixtures;
using Microsoft.AspNetCore.Identity;

namespace FleetLedger.Api.Tests.Common.Security;

public class IdentityCredentialVerifierTests
{
    private const string Email = "gestor@fleetledger.dev";
    private const string Password = "Senha-Forte-123";

    private readonly UserManager<User> _userManager = UserManagerFactory.Create(new FakeUserStore());
    private readonly IdentityCredentialVerifier _verifier;
    private readonly User _administrator = new() { UserName = Email, Email = Email };

    public IdentityCredentialVerifierTests()
    {
        _verifier = new IdentityCredentialVerifier(_userManager);
        _userManager.CreateAsync(_administrator, Password).GetAwaiter().GetResult();
    }

    [Fact]
    public async Task VerifyAsync_WithRightCredentials_ReturnsUserId()
        => (await _verifier.VerifyAsync(Email, Password)).Should().Be(CredentialVerification.Verified(_administrator.Id));

    [Fact]
    public async Task VerifyAsync_IgnoresEmailCase()
        => (await _verifier.VerifyAsync("GESTOR@FleetLedger.dev", Password)).IsVerified.Should().BeTrue();

    [Fact]
    public async Task VerifyAsync_WithUnknownEmail_RefusesAsUnknownEmail()
        => (await _verifier.VerifyAsync("outro@fleetledger.dev", Password)).Refusal.Should().Be(ELoginRefusal.UnknownEmail);

    [Fact]
    public async Task VerifyAsync_WithWrongPassword_RefusesAndCountsTheFailure()
    {
        CredentialVerification verification = await _verifier.VerifyAsync(Email, "senha-errada");

        verification.Refusal.Should().Be(ELoginRefusal.WrongPassword);
        _administrator.AccessFailedCount.Should().Be(1);
    }

    [Fact]
    public async Task VerifyAsync_AfterMaxWrongPasswords_LocksOut()
    {
        CredentialVerification last = await FailRepeatedlyAsync(AdministratorIdentityPolicy.MaxFailedAccessAttempts);

        last.Refusal.Should().Be(ELoginRefusal.LockedOut);
    }

    [Fact]
    public async Task VerifyAsync_WhenLockedOut_RefusesEvenTheRightPassword()
    {
        await FailRepeatedlyAsync(AdministratorIdentityPolicy.MaxFailedAccessAttempts);

        (await _verifier.VerifyAsync(Email, Password)).Refusal.Should().Be(ELoginRefusal.LockedOut);
    }

    [Fact]
    public async Task VerifyAsync_WithRightPasswordAfterFailures_ResetsTheCounter()
    {
        await FailRepeatedlyAsync(AdministratorIdentityPolicy.MaxFailedAccessAttempts - 1);

        await _verifier.VerifyAsync(Email, Password);

        _administrator.AccessFailedCount.Should().Be(0);
    }

    private async Task<CredentialVerification> FailRepeatedlyAsync(int attempts)
    {
        CredentialVerification last = CredentialVerification.Refused(ELoginRefusal.None);
        for (int attempt = 0; attempt < attempts; attempt++)
            last = await _verifier.VerifyAsync(Email, "senha-errada");

        return last;
    }
}
