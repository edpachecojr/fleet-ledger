using FleetLedger.Core.Models;
using FleetLedger.Core.Responses;

namespace FleetLedger.Api.Handlers.Vehicles;

public static class VehicleResponses
{
    public static Response<Vehicle?> Created(Vehicle vehicle) => new(vehicle, StatusCodes.Status201Created);

    public static Response<Vehicle?> Found(Vehicle vehicle) => new(vehicle);

    public static Response<Vehicle?> Updated(Vehicle vehicle) => new(vehicle, message: "Veículo atualizado.");

    public static Response<Vehicle?> NotFound(Guid vehicleId)
        => new(null, StatusCodes.Status404NotFound, $"Veículo {vehicleId} não encontrado.");

    public static Response<Vehicle?> DuplicatePlate(string plate)
        => new(null, StatusCodes.Status409Conflict, $"Já existe um veículo com a placa {plate}.");

    public static Response<Vehicle?> StatusChangeBlockedOnTrip(string plate)
        => new(null, StatusCodes.Status409Conflict,
            $"O veículo {plate} está em viagem: o status só muda pelo fechamento ou cancelamento da viagem.");

    public static Response<Vehicle?> StatusChangeBlockedInactive(string plate)
        => new(null, StatusCodes.Status409Conflict,
            $"O veículo {plate} está inativo. Reative-o antes de alterar o status operacional.");
}
