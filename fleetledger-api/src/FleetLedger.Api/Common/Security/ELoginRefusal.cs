namespace FleetLedger.Api.Common.Security;

// Motivo fica só no log: para o cliente, toda recusa tem a mesma mensagem genérica.
public enum ELoginRefusal
{
    None = 0,
    UnknownEmail = 1,
    WrongPassword = 2,
    LockedOut = 3
}
