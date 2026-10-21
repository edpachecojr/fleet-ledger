namespace FleetLedger.Core.Models;

/// <summary>Categoria da CNH (Código de Trânsito Brasileiro).</summary>
public enum ECnhCategory
{
    /// <summary>Motos.</summary>
    A = 1,

    /// <summary>Carros.</summary>
    B = 2,

    /// <summary>Carga acima de 3,5 t.</summary>
    C = 3,

    /// <summary>Passageiros.</summary>
    D = 4,

    /// <summary>Combinação de veículos (carreta).</summary>
    E = 5,

    /// <summary>A e B.</summary>
    AB = 6,

    /// <summary>A e C.</summary>
    AC = 7,

    /// <summary>A e D.</summary>
    AD = 8,

    /// <summary>A e E.</summary>
    AE = 9
}
