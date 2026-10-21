namespace FleetLedger.Api.Handlers;

// Campo opcional em branco é "não informado": grava null, não string vazia.
public static class OptionalText
{
    public static string? TrimToNull(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
