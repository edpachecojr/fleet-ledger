using FleetLedger.Core.Responses;
using FleetLedger.Core.Responses.Auth;

namespace FleetLedger.Api.Handlers.Auth;

public static class AuthResponses
{
    // Mesma mensagem para e-mail inexistente, senha errada e bloqueio: não revela
    // qual e-mail existe nem que a conta está bloqueada.
    public const string InvalidCredentialsMessage = "E-mail ou senha inválidos.";

    public static Response<IssuedAccessToken?> InvalidCredentials()
        => new(null, StatusCodes.Status401Unauthorized, InvalidCredentialsMessage);

    public static Response<IssuedAccessToken?> LoggedIn(IssuedAccessToken accessToken)
        => new(accessToken);
}
