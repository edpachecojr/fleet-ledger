using System.Text;
using FleetLedger.Core.Common.Validation;
using Microsoft.Extensions.Options;

namespace FleetLedger.Api.Options;

public sealed class JwtOptionsValidator : IValidateOptions<JwtOptions>
{
    private const string Section = JwtOptions.SectionName;

    public ValidateOptionsResult Validate(string? name, JwtOptions options)
    {
        int signingKeyBytes = Encoding.UTF8.GetByteCount(options.SigningKey);

        return new ValidationBuilder()
            .Required($"{Section}:Issuer", options.Issuer, $"{Section}:Issuer é obrigatório.")
            .Required($"{Section}:Audience", options.Audience, $"{Section}:Audience é obrigatório.")
            .When(
                signingKeyBytes < JwtOptions.MinimumSigningKeyBytes,
                $"{Section}:SigningKey",
                $"{Section}:SigningKey deve ter ao menos {JwtOptions.MinimumSigningKeyBytes} bytes. " +
                $"Recebido: {signingKeyBytes} bytes.")
            .Range(
                $"{Section}:AccessTokenLifetimeMinutes",
                options.AccessTokenLifetimeMinutes,
                JwtOptions.MinimumAccessTokenLifetimeMinutes,
                JwtOptions.MaximumAccessTokenLifetimeMinutes,
                $"{Section}:AccessTokenLifetimeMinutes deve estar entre {JwtOptions.MinimumAccessTokenLifetimeMinutes} " +
                $"e {JwtOptions.MaximumAccessTokenLifetimeMinutes}. Recebido: {options.AccessTokenLifetimeMinutes}.")
            .Build()
            .ToOptionsResult();
    }
}
