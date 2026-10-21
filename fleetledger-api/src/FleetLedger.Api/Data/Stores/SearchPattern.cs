namespace FleetLedger.Api.Data.Stores;

public static class SearchPattern
{
    public const string EscapeCharacter = "\\";

    // Padrão "contém" para ILIKE, com % e _ do usuário escapados: buscar "10%" não vira curinga.
    public static string Contains(string term)
        => "%" + term.Replace(EscapeCharacter, EscapeCharacter + EscapeCharacter)
            .Replace("%", EscapeCharacter + "%")
            .Replace("_", EscapeCharacter + "_") + "%";
}
