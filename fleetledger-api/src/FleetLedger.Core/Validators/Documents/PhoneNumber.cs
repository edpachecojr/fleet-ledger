namespace FleetLedger.Core.Validators.Documents;

/// <summary>Telefone brasileiro com DDD: 10 dígitos (fixo) ou 11 (celular), sem máscara.</summary>
/// <example><code>PhoneNumber.IsValid(DigitsOnly.Of("(11) 98765-4321")) // true</code></example>
public static class PhoneNumber
{
    /// <summary>Fixo com DDD.</summary>
    public const int LandlineLength = 10;

    /// <summary>Celular com DDD; também o tamanho da coluna.</summary>
    public const int MobileLength = 11;

    /// <summary>Verdadeiro para 10 ou 11 dígitos.</summary>
    public static bool IsValid(string digits)
        => digits.Length is LandlineLength or MobileLength && digits.All(char.IsAsciiDigit);
}
