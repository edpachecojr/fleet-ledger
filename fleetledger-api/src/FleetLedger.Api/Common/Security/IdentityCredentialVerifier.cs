using FleetLedger.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace FleetLedger.Api.Common.Security;

// Confere e-mail e senha pelo Identity e mantém o contador de bloqueio
// (AdministratorIdentityPolicy): senha errada soma, senha certa zera.
public sealed class IdentityCredentialVerifier(UserManager<User> userManager) : ICredentialVerifier
{
    public async Task<CredentialVerification> VerifyAsync(string email, string password)
    {
        User? user = await userManager.FindByEmailAsync(email);
        if (user is null)
            return CredentialVerification.Refused(ELoginRefusal.UnknownEmail);

        // Antes da senha: bloqueado não testa senha, senão o bloqueio não freia força bruta.
        if (await userManager.IsLockedOutAsync(user))
            return CredentialVerification.Refused(ELoginRefusal.LockedOut);

        if (!await userManager.CheckPasswordAsync(user, password))
            return await RegisterWrongPasswordAsync(user);

        await userManager.ResetAccessFailedCountAsync(user);
        return CredentialVerification.Verified(user.Id);
    }

    private async Task<CredentialVerification> RegisterWrongPasswordAsync(User user)
    {
        await userManager.AccessFailedAsync(user);

        return CredentialVerification.Refused(
            await userManager.IsLockedOutAsync(user) ? ELoginRefusal.LockedOut : ELoginRefusal.WrongPassword);
    }
}
