using FleetLedger.Api.Data.Stores;
using FleetLedger.Api.Data.Stores.Vehicles;
using FleetLedger.Core.Handlers;
using FleetLedger.Core.Models;
using FleetLedger.Core.Requests;
using FleetLedger.Core.Requests.Vehicles;
using FleetLedger.Core.Responses;
using FleetLedger.Core.Validators.Vehicles;

namespace FleetLedger.Api.Handlers.Vehicles;

public sealed class VehicleHandler(
    IVehicleStore store,
    TimeProvider timeProvider,
    ILogger<VehicleHandler> logger) : IVehicleHandler
{
    public async Task<Response<Vehicle?>> CreateAsync(CreateVehicleRequest request, CancellationToken cancellationToken)
    {
        string plate = VehiclePlate.Normalize(request.Plate);
        if (await store.PlateExistsAsync(request.OrganizationId, plate, cancellationToken))
            return VehicleResponses.DuplicatePlate(plate);

        Vehicle vehicle = NewVehicle(request, plate);
        store.Add(vehicle);
        await store.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Veículo {VehicleId} cadastrado na organização {OrganizationId}", vehicle.Id, vehicle.OrganizationId);
        return VehicleResponses.Created(vehicle);
    }

    public async Task<Response<Vehicle?>> UpdateAsync(UpdateVehicleRequest request, CancellationToken cancellationToken)
    {
        Vehicle? vehicle = await store.FindAsync(request.OrganizationId, request.Id, cancellationToken);
        if (vehicle is null)
            return VehicleResponses.NotFound(request.Id);

        vehicle.Model = request.Model.Trim();
        vehicle.CapacityKg = request.CapacityKg;
        vehicle.MaintenanceProvisionRatePerKm = request.MaintenanceProvisionRatePerKm;
        vehicle.UpdatedAt = timeProvider.GetUtcNow();
        await store.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Veículo {VehicleId} atualizado", vehicle.Id);
        return VehicleResponses.Updated(vehicle);
    }

    public async Task<Response<Vehicle?>> GetAsync(ByIdRequest request, CancellationToken cancellationToken)
    {
        Vehicle? vehicle = await store.FindAsync(request.OrganizationId, request.Id, cancellationToken);
        return vehicle is null ? VehicleResponses.NotFound(request.Id) : VehicleResponses.Found(vehicle);
    }

    public async Task<PagedResponse<List<Vehicle>>> ListAsync(ListVehiclesRequest request, CancellationToken cancellationToken)
    {
        StorePage<Vehicle> page = await store.ListAsync(request, cancellationToken);
        return new PagedResponse<List<Vehicle>>(page.Items, page.TotalCount, request.EffectivePageNumber, request.EffectivePageSize);
    }

    public async Task<Response<Vehicle?>> ChangeStatusAsync(ChangeVehicleStatusRequest request, CancellationToken cancellationToken)
    {
        Vehicle? vehicle = await store.FindAsync(request.OrganizationId, request.Id, cancellationToken);
        if (vehicle is null)
            return VehicleResponses.NotFound(request.Id);

        EVehicleStatusChangeVerdict verdict = VehicleStatusTransition.Evaluate(vehicle, request.Status);
        if (verdict != EVehicleStatusChangeVerdict.Allowed)
            return RefusedStatusChange(vehicle, verdict);

        EVehicleStatus previous = vehicle.Status;
        vehicle.Status = request.Status;
        vehicle.UpdatedAt = timeProvider.GetUtcNow();
        await store.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Veículo {VehicleId} passou de {PreviousStatus} para {VehicleStatus}", vehicle.Id, previous, vehicle.Status);
        return VehicleResponses.Found(vehicle);
    }

    public async Task<Response<Vehicle?>> DeactivateAsync(ByIdRequest request, CancellationToken cancellationToken)
        => await ChangeActivityAsync(request, deactivate: true, cancellationToken);

    public async Task<Response<Vehicle?>> ReactivateAsync(ByIdRequest request, CancellationToken cancellationToken)
        => await ChangeActivityAsync(request, deactivate: false, cancellationToken);

    // Idempotente: inativar um inativo (ou reativar um ativo) devolve o veículo sem gravar.
    private async Task<Response<Vehicle?>> ChangeActivityAsync(ByIdRequest request, bool deactivate, CancellationToken cancellationToken)
    {
        Vehicle? vehicle = await store.FindAsync(request.OrganizationId, request.Id, cancellationToken);
        if (vehicle is null)
            return VehicleResponses.NotFound(request.Id);
        if (vehicle.IsActive != deactivate)
            return VehicleResponses.Found(vehicle);

        DateTimeOffset now = timeProvider.GetUtcNow();
        vehicle.DeactivatedAt = deactivate ? now : null;
        vehicle.UpdatedAt = now;
        await store.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Veículo {VehicleId} {VehicleActivity}", vehicle.Id, deactivate ? "inativado" : "reativado");
        return VehicleResponses.Found(vehicle);
    }

    private Response<Vehicle?> RefusedStatusChange(Vehicle vehicle, EVehicleStatusChangeVerdict verdict)
    {
        if (verdict == EVehicleStatusChangeVerdict.Unchanged)
            return VehicleResponses.Found(vehicle);

        logger.LogInformation("Troca de status do veículo {VehicleId} recusada: {StatusChangeVerdict}", vehicle.Id, verdict);
        return verdict == EVehicleStatusChangeVerdict.BlockedOnTrip
            ? VehicleResponses.StatusChangeBlockedOnTrip(vehicle.Plate)
            : VehicleResponses.StatusChangeBlockedInactive(vehicle.Plate);
    }

    private Vehicle NewVehicle(CreateVehicleRequest request, string plate)
    {
        DateTimeOffset now = timeProvider.GetUtcNow();
        return new Vehicle
        {
            OrganizationId = request.OrganizationId,
            Plate = plate,
            Model = request.Model.Trim(),
            CapacityKg = request.CapacityKg,
            InitialOdometerKm = request.InitialOdometerKm,
            CurrentOdometerKm = request.InitialOdometerKm,
            MaintenanceProvisionRatePerKm = request.MaintenanceProvisionRatePerKm,
            Status = EVehicleStatus.Available,
            CreatedAt = now,
            UpdatedAt = now
        };
    }
}
