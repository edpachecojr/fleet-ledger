namespace FleetLedger.Core.Models;

/// <summary>
/// Situação dos documentos do motorista (CNH e exame toxicológico), pelo pior dos dois.
/// Só gera aviso na abertura de viagem, nunca bloqueio (Q1).
/// </summary>
public enum EDocumentStatus
{
    /// <summary>Tudo válido por mais de 30 dias.</summary>
    Regular = 1,

    /// <summary>Algum documento vence de hoje até daqui a 30 dias.</summary>
    ExpiringSoon = 2,

    /// <summary>Algum documento venceu antes de hoje.</summary>
    Expired = 3
}
