using FleetLedger.Core.Common.Validation;
using FleetLedger.Core.Requests.Auth;

namespace FleetLedger.Core.Validators.Auth;

/// <summary>
/// Só a forma das credenciais: se elas conferem é decisão do handler, com mensagem genérica.
/// </summary>
/// <example><code>ValidationResult result = new LoginValidator().Validate(request);</code></example>
public sealed class LoginValidator : Validator<LoginRequest>
{
    /// <summary>Mesmo limite da coluna <c>email</c> do Identity.</summary>
    public const int EmailMaxLength = 256;

    /// <summary>Teto que impede corpo gigante de chegar ao hash da senha.</summary>
    public const int PasswordMaxLength = 128;

    /// <inheritdoc />
    protected override void Rules(LoginRequest request, ValidationBuilder rules)
        => rules
            .Required(nameof(request.Email), request.Email, "E-mail é obrigatório")
            .MaxLength(nameof(request.Email), request.Email, EmailMaxLength,
                $"E-mail deve ter até {EmailMaxLength} caracteres")
            .Required(nameof(request.Password), request.Password, "Senha é obrigatória")
            .MaxLength(nameof(request.Password), request.Password, PasswordMaxLength,
                $"Senha deve ter até {PasswordMaxLength} caracteres");
}
