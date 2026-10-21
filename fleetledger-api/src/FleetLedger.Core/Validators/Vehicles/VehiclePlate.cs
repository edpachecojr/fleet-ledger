using System.Text.RegularExpressions;

namespace FleetLedger.Core.Validators.Vehicles;

/// <summary>
/// Formato da placa brasileira: antigo (ABC1234) ou Mercosul (ABC1D23). A placa é guardada
/// normalizada, então "abc-1234" e "ABC1234" são a mesma placa.
/// </summary>
/// <example><code>VehiclePlate.IsValid(VehiclePlate.Normalize("abc-1d23")) // true</code></example>
public static partial class VehiclePlate
{
    /// <summary>Tamanho da placa normalizada.</summary>
    public const int Length = 7;

    /// <summary>Maiúsculas, sem hífen nem espaços; nulo vira vazio.</summary>
    public static string Normalize(string? plate)
        => (plate ?? string.Empty).Replace("-", string.Empty).Replace(" ", string.Empty).ToUpperInvariant();

    /// <summary>Verdadeiro para placa normalizada no formato antigo ou Mercosul.</summary>
    public static bool IsValid(string normalizedPlate) => PlatePattern().IsMatch(normalizedPlate);

    // Quarta posição sempre dígito; a quinta é dígito (antiga) ou letra (Mercosul).
    [GeneratedRegex("^[A-Z]{3}[0-9][A-Z0-9][0-9]{2}$")]
    private static partial Regex PlatePattern();
}
