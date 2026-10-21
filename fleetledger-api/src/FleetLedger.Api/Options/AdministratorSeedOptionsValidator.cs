using FleetLedger.Api.Data.Mappings;
using FleetLedger.Core.Common.Validation;
using FleetLedger.Core.Validators.Auth;
using Microsoft.Extensions.Options;

namespace FleetLedger.Api.Options;

public sealed class AdministratorSeedOptionsValidator : IValidateOptions<AdministratorSeedOptions>
{
    private const string Section = AdministratorSeedOptions.SectionName;

    public ValidateOptionsResult Validate(string? name, AdministratorSeedOptions options)
        => new ValidationBuilder()
            .Required($"{Section}:OrganizationName", options.OrganizationName,
                $"{Section}:OrganizationName é obrigatório.")
            .MaxLength($"{Section}:OrganizationName", options.OrganizationName, OrganizationMapping.NameMaxLength,
                $"{Section}:OrganizationName deve ter até {OrganizationMapping.NameMaxLength} caracteres. " +
                $"Recebido: {options.OrganizationName.Length}.")
            .When(!IsEmail(options.Email), $"{Section}:Email",
                $"{Section}:Email inválido. Recebido: '{options.Email}'. Esperado: e-mail com '@', " +
                $"até {LoginValidator.EmailMaxLength} caracteres.")
            .When(!HasAcceptedPasswordLength(options.Password), $"{Section}:Password",
                $"{Section}:Password deve ter de {AdministratorSeedOptions.MinimumPasswordLength} a " +
                $"{LoginValidator.PasswordMaxLength} caracteres. Recebido: {options.Password.Length} caracteres.")
            .Build()
            .ToOptionsResult();

    // O teto é o mesmo do login: senha mais longa seria criada e nunca aceita.
    private static bool HasAcceptedPasswordLength(string password)
        => password.Length is >= AdministratorSeedOptions.MinimumPasswordLength and <= LoginValidator.PasswordMaxLength;

    private static bool IsEmail(string email)
        => email.Length <= LoginValidator.EmailMaxLength && email.IndexOf('@') > 0 && !email.EndsWith('@');
}
