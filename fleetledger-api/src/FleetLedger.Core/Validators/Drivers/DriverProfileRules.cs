using FleetLedger.Core.Common.Validation;
using FleetLedger.Core.Requests.Drivers;
using FleetLedger.Core.Validators.Documents;

namespace FleetLedger.Core.Validators.Drivers;

/// <summary>Regras dos campos editáveis do motorista, comuns ao cadastro e à edição.</summary>
/// <example><code>rules.DriverProfile(request);</code></example>
public static class DriverProfileRules
{
    /// <summary>Limite do nome.</summary>
    public const int NameMaxLength = 160;

    /// <summary>Dígitos do número da CNH (registro nacional).</summary>
    public const int CnhNumberLength = 11;

    /// <summary>Limite do texto de dados bancários/PIX.</summary>
    public const int BankDetailsMaxLength = 500;

    /// <summary>Casas decimais da comissão.</summary>
    public const int CommissionDecimalPlaces = 2;

    /// <summary>Teto da comissão, em %.</summary>
    public const decimal MaxCommissionPercent = 100m;

    /// <summary>Nome, telefone, CNH (número, categoria, validade) e comissão entre 0% e 100%.</summary>
    public static ValidationBuilder DriverProfile(this ValidationBuilder rules, DriverProfileFields profile)
        => rules
            .Required("Name", profile.Name, "Nome é obrigatório")
            .MaxLength("Name", profile.Name, NameMaxLength, $"Nome deve ter até {NameMaxLength} caracteres")
            .Required("Phone", profile.Phone, "Telefone é obrigatório")
            .When(!string.IsNullOrWhiteSpace(profile.Phone) && !PhoneNumber.IsValid(DigitsOnly.Of(profile.Phone)), "Phone",
                $"Telefone '{profile.Phone}' inválido. Esperado: DDD + número, com 10 ou 11 dígitos")
            .When(DigitsOnly.Of(profile.CnhNumber).Length != CnhNumberLength, "CnhNumber",
                $"Número da CNH '{profile.CnhNumber}' inválido. Esperado: {CnhNumberLength} dígitos")
            .When(!Enum.IsDefined(profile.CnhCategory), "CnhCategory",
                $"Categoria da CNH '{profile.CnhCategory}' inválida. Esperado: A, B, C, D, E, AB, AC, AD ou AE")
            .When(profile.CnhExpiresOn == default, "CnhExpiresOn", "Validade da CNH é obrigatória")
            .Commission(profile.DefaultCommissionPercent)
            .MaxLength("BankDetails", profile.BankDetails, BankDetailsMaxLength,
                $"Dados bancários devem ter até {BankDetailsMaxLength} caracteres");

    private static ValidationBuilder Commission(this ValidationBuilder rules, decimal percent)
        => rules
            .When(percent is < 0 or > MaxCommissionPercent, "DefaultCommissionPercent",
                $"Comissão deve estar entre 0% e {MaxCommissionPercent}% (recebido: {percent}%)")
            .When(decimal.Round(percent, CommissionDecimalPlaces) != percent, "DefaultCommissionPercent",
                $"Comissão deve ter até {CommissionDecimalPlaces} casas decimais (recebido: {percent}%)");
}
