using FleetLedger.Core.Models;
using FleetLedger.Core.Responses;

namespace FleetLedger.Api.Handlers.Drivers;

public static class DriverResponses
{
    public static Response<Driver?> Created(Driver driver) => new(driver, StatusCodes.Status201Created);

    public static Response<Driver?> Found(Driver driver) => new(driver);

    public static Response<Driver?> Updated(Driver driver) => new(driver, message: "Motorista atualizado.");

    public static Response<Driver?> NotFound(Guid driverId)
        => new(null, StatusCodes.Status404NotFound, $"Motorista {driverId} não encontrado.");

    public static Response<Driver?> DuplicateCpf(string cpf)
        => new(null, StatusCodes.Status409Conflict, $"Já existe um motorista com o CPF {cpf}.");

    public static Response<Driver?> DuplicateCnhNumber(string cnhNumber)
        => new(null, StatusCodes.Status409Conflict, $"Já existe um motorista com a CNH {cnhNumber}.");
}
