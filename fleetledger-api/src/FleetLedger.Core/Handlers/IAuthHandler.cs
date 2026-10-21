using FleetLedger.Core.Requests.Auth;
using FleetLedger.Core.Responses;
using FleetLedger.Core.Responses.Auth;

namespace FleetLedger.Core.Handlers;

/// <summary>Autenticação do administrador.</summary>
public interface IAuthHandler
{
    /// <summary>
    /// Confere as credenciais e emite o access token. Falha sempre com 401 e a mesma
    /// mensagem, exista ou não o e-mail, esteja ou não bloqueado.
    /// </summary>
    /// <example><code>Response&lt;IssuedAccessToken?&gt; result = await handler.LoginAsync(request, cancellationToken);</code></example>
    Task<Response<IssuedAccessToken?>> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
}
