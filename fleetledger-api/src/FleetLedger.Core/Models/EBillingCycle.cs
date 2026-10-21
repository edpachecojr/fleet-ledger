namespace FleetLedger.Core.Models;

/// <summary>Periodicidade com que o cliente é faturado.</summary>
public enum EBillingCycle
{
    /// <summary>Fatura toda semana.</summary>
    Weekly = 1,

    /// <summary>Fatura a cada quinze dias.</summary>
    Biweekly = 2,

    /// <summary>Fatura uma vez por mês.</summary>
    Monthly = 3
}
