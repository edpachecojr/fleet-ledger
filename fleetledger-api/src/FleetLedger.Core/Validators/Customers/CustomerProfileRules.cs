using FleetLedger.Core.Common.Validation;
using FleetLedger.Core.Models;
using FleetLedger.Core.Validators.Documents;

namespace FleetLedger.Core.Validators.Customers;

/// <summary>Campos editáveis do cliente, comuns ao cadastro e à edição.</summary>
/// <example><code>rules.CustomerProfile(new(request.CorporateName, request.TradeName, request.Phone, request.Email, request.BillingCycle, request.PaymentTermDays));</code></example>
public static class CustomerProfileRules
{
    /// <summary>Limite de razão social e nome fantasia.</summary>
    public const int NameMaxLength = 160;

    /// <summary>Limite prático de e-mail (RFC 5321).</summary>
    public const int EmailMaxLength = 254;

    /// <summary>Os campos validados, já que cadastro e edição são requisições diferentes.</summary>
    public sealed record CustomerProfile(
        string? CorporateName,
        string? TradeName,
        string? Phone,
        string? Email,
        EBillingCycle BillingCycle,
        int? PaymentTermDays);

    /// <summary>Razão social e ciclo obrigatórios; opcionais validados só quando informados.</summary>
    public static ValidationBuilder CustomerProfileFields(this ValidationBuilder rules, CustomerProfile profile)
        => rules
            .Required("CorporateName", profile.CorporateName, "Razão social é obrigatória")
            .MaxLength("CorporateName", profile.CorporateName, NameMaxLength, $"Razão social deve ter até {NameMaxLength} caracteres")
            .MaxLength("TradeName", profile.TradeName, NameMaxLength, $"Nome fantasia deve ter até {NameMaxLength} caracteres")
            .When(!string.IsNullOrWhiteSpace(profile.Phone) && !PhoneNumber.IsValid(DigitsOnly.Of(profile.Phone)), "Phone",
                $"Telefone '{profile.Phone}' inválido. Esperado: DDD + número, com 10 ou 11 dígitos")
            .When(!string.IsNullOrWhiteSpace(profile.Email) && !LooksLikeEmail(profile.Email.Trim()), "Email",
                $"E-mail '{profile.Email}' inválido. Esperado: nome@dominio.com.br")
            .MaxLength("Email", profile.Email, EmailMaxLength, $"E-mail deve ter até {EmailMaxLength} caracteres")
            .When(!Enum.IsDefined(profile.BillingCycle), "BillingCycle",
                $"Ciclo de faturamento '{profile.BillingCycle}' inválido. Esperado: Weekly, Biweekly ou Monthly")
            .When(profile.PaymentTermDays is < 1, "PaymentTermDays",
                $"Prazo de pagamento deve ser de 1 dia ou mais (recebido: {profile.PaymentTermDays})");

    // Forma mínima (algo@algo.algo, sem espaço): a prova de que o e-mail existe é outra conversa.
    private static bool LooksLikeEmail(string email)
    {
        int at = email.IndexOf('@');
        return at > 0
            && at == email.LastIndexOf('@')
            && !email.Any(char.IsWhiteSpace)
            && email.IndexOf('.', at) > at + 1
            && !email.EndsWith('.');
    }
}
