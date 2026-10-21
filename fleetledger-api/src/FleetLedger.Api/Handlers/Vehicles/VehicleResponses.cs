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
}
