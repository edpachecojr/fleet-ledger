namespace FleetLedger.Api.Options;

public sealed class CalendarOptions
{
    public const string SectionName = "Calendar";

    // "Hoje" da operação (vencimento de documentos, datas de viagem) segue o fuso da empresa,
    // não o UTC do servidor (decisão de 2026-10-21).
    public string TimeZone { get; init; } = "America/Sao_Paulo";
}
