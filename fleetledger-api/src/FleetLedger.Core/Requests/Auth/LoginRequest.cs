namespace FleetLedger.Core.Requests.Auth;

/// <summary>Credenciais do administrador para obter um access token.</summary>
/// <remarks>
/// Não herda <see cref="Request"/>: é a única requisição anônima, e ainda não existe
/// organização a atribuir.
/// </remarks>
/// <example><code>new LoginRequest { Email = "gestor@empresa.com.br", Password = "..." }</code></example>
public sealed class LoginRequest
{
    /// <summary>E-mail cadastrado do administrador.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Senha em texto, só em trânsito (HTTPS); nunca é registrada em log.</summary>
    public string Password { get; set; } = string.Empty;
}
