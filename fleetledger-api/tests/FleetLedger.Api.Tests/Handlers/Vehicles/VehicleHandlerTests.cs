using FleetLedger.Api.Handlers.Vehicles;
using FleetLedger.Api.Tests.Fakes;
using FleetLedger.Api.Tests.Fixtures;
using FleetLedger.Core.Models;
using FleetLedger.Core.Requests;
using FleetLedger.Core.Requests.Vehicles;
using FleetLedger.Core.Responses;
using Microsoft.Extensions.Logging.Abstractions;

namespace FleetLedger.Api.Tests.Handlers.Vehicles;

public class VehicleHandlerTests
{
    private readonly Guid _organizationId = Guid.CreateVersion7();
    private readonly Guid _otherOrganizationId = Guid.CreateVersion7();
    private readonly FakeVehicleStore _store = new();
    private readonly VehicleHandler _handler;

    public VehicleHandlerTests()
        => _handler = new VehicleHandler(_store, new FakeTimeProvider(), NullLogger<VehicleHandler>.Instance);

    [Fact]
    public async Task CreateAsync_WithNewPlate_StoresAvailableVehicleWithCurrentOdometerEqualToInitial()
    {
        Response<Vehicle?> result = await _handler.CreateAsync(CreateRequest("abc-1d23"), CancellationToken.None);

        result.Code.Should().Be(201);
        result.Data.Should().BeEquivalentTo(new
        {
            Plate = "ABC1D23",
            CurrentOdometerKm = 120000,
            Status = EVehicleStatus.Available,
            IsActive = true,
            OrganizationId = _organizationId,
            CreatedAt = FakeTimeProvider.ReferenceNow
        });
        _store.SaveCount.Should().Be(1);
    }

    [Fact]
    public async Task CreateAsync_WhenPlateExistsInTheOrganization_Returns409WithThePlate()
    {
        _store.With(VehicleFactory.Create(_organizationId, "ABC1D23"));

        Response<Vehicle?> result = await _handler.CreateAsync(CreateRequest("ABC-1D23"), CancellationToken.None);

        result.Should().BeEquivalentTo(new { Code = 409, Data = (Vehicle?)null });
        result.Message.Should().Contain("ABC1D23");
        _store.SaveCount.Should().Be(0);
    }

    [Fact]
    public async Task CreateAsync_WhenPlateExistsOnlyInAnotherOrganization_Creates()
    {
        _store.With(VehicleFactory.Create(_otherOrganizationId, "ABC1D23"));

        Response<Vehicle?> result = await _handler.CreateAsync(CreateRequest("ABC1D23"), CancellationToken.None);

        result.Code.Should().Be(201);
    }

    [Fact]
    public async Task UpdateAsync_ChangesOnlyEditableFields()
    {
        Vehicle vehicle = _store.With(VehicleFactory.Create(_organizationId)).Vehicles[0];

        Response<Vehicle?> result = await _handler.UpdateAsync(
            new UpdateVehicleRequest { Id = vehicle.Id, OrganizationId = _organizationId, Model = "Scania R 450", CapacityKg = 30000, MaintenanceProvisionRatePerKm = 0.2m },
            CancellationToken.None);

        result.Data.Should().BeEquivalentTo(new
        {
            Plate = "ABC1D23",
            Model = "Scania R 450",
            CapacityKg = 30000,
            MaintenanceProvisionRatePerKm = 0.2m,
            CurrentOdometerKm = 125000,
            UpdatedAt = FakeTimeProvider.ReferenceNow
        });
    }

    [Fact]
    public async Task GetAsync_WhenVehicleDoesNotExist_Returns404WithTheId()
    {
        Guid missingId = Guid.CreateVersion7();

        Response<Vehicle?> result = await _handler.GetAsync(ById(missingId, _organizationId), CancellationToken.None);

        result.Code.Should().Be(404);
        result.Message.Should().Contain(missingId.ToString());
    }

    [Fact]
    public async Task Operations_OnAnotherOrganizationsVehicle_Return404AndChangeNothing()
    {
        Vehicle foreign = _store.With(VehicleFactory.Create(_otherOrganizationId)).Vehicles[0];

        Response<Vehicle?> get = await _handler.GetAsync(ById(foreign.Id, _organizationId), CancellationToken.None);
        Response<Vehicle?> update = await _handler.UpdateAsync(
            new UpdateVehicleRequest { Id = foreign.Id, OrganizationId = _organizationId, Model = "Outro", CapacityKg = 1 },
            CancellationToken.None);
        Response<Vehicle?> deactivate = await _handler.DeactivateAsync(ById(foreign.Id, _organizationId), CancellationToken.None);

        new[] { get.Code, update.Code, deactivate.Code }.Should().OnlyContain(code => code == 404);
        foreign.Should().BeEquivalentTo(new { Model = "Volvo FH 540", IsActive = true });
    }

    [Fact]
    public async Task ListAsync_ReturnsOnlyTheOrganizationsVehiclesWithEffectivePaging()
    {
        _store.With(VehicleFactory.Create(_organizationId, "AAA1111")).With(VehicleFactory.Create(_otherOrganizationId, "BBB2222"));

        PagedResponse<List<Vehicle>> result = await _handler.ListAsync(
            new ListVehiclesRequest { OrganizationId = _organizationId, PageNumber = 0, PageSize = 500 },
            CancellationToken.None);

        result.Data!.Select(vehicle => vehicle.Plate).Should().Equal("AAA1111");
        result.Should().BeEquivalentTo(new { TotalCount = 1, CurrentPage = 1, PageSize = 100 });
    }

    [Theory]
    [InlineData(EVehicleStatus.Available)]
    [InlineData(EVehicleStatus.OnTrip)]
    [InlineData(EVehicleStatus.InMaintenance)]
    public async Task DeactivateAsync_InAnyStatus_DeactivatesAndKeepsTheStatus(EVehicleStatus status)
    {
        Vehicle vehicle = _store.With(VehicleFactory.Create(_organizationId, status: status)).Vehicles[0];

        Response<Vehicle?> result = await _handler.DeactivateAsync(ById(vehicle.Id, _organizationId), CancellationToken.None);

        result.Data.Should().BeEquivalentTo(new { IsActive = false, DeactivatedAt = FakeTimeProvider.ReferenceNow, Status = status });
    }

    [Fact]
    public async Task DeactivateAsync_WhenAlreadyInactive_DoesNotSaveAgain()
    {
        Vehicle vehicle = _store.With(VehicleFactory.Create(_organizationId, deactivatedAt: FakeTimeProvider.ReferenceNow.AddDays(-1))).Vehicles[0];

        await _handler.DeactivateAsync(ById(vehicle.Id, _organizationId), CancellationToken.None);

        _store.SaveCount.Should().Be(0);
        vehicle.DeactivatedAt.Should().Be(FakeTimeProvider.ReferenceNow.AddDays(-1));
    }

    [Fact]
    public async Task ReactivateAsync_WhenInactive_ClearsTheDeactivation()
    {
        Vehicle vehicle = _store.With(VehicleFactory.Create(_organizationId, deactivatedAt: FakeTimeProvider.ReferenceNow.AddDays(-1))).Vehicles[0];

        Response<Vehicle?> result = await _handler.ReactivateAsync(ById(vehicle.Id, _organizationId), CancellationToken.None);

        result.Data.Should().BeEquivalentTo(new { IsActive = true, DeactivatedAt = (DateTimeOffset?)null });
    }

    [Fact]
    public async Task ChangeStatusAsync_FromAvailableToMaintenance_SavesTheNewStatus()
    {
        Vehicle vehicle = _store.With(VehicleFactory.Create(_organizationId)).Vehicles[0];

        Response<Vehicle?> result = await _handler.ChangeStatusAsync(StatusRequest(vehicle.Id, EVehicleStatus.InMaintenance), CancellationToken.None);

        result.Data.Should().BeEquivalentTo(new { Status = EVehicleStatus.InMaintenance, UpdatedAt = FakeTimeProvider.ReferenceNow });
        _store.SaveCount.Should().Be(1);
    }

    [Fact]
    public async Task ChangeStatusAsync_WhenOnTrip_Returns409AndKeepsTheStatus()
    {
        Vehicle vehicle = _store.With(VehicleFactory.Create(_organizationId, status: EVehicleStatus.OnTrip)).Vehicles[0];

        Response<Vehicle?> result = await _handler.ChangeStatusAsync(StatusRequest(vehicle.Id, EVehicleStatus.InMaintenance), CancellationToken.None);

        result.Code.Should().Be(409);
        result.Message.Should().Contain("em viagem");
        vehicle.Status.Should().Be(EVehicleStatus.OnTrip);
    }

    [Fact]
    public async Task ChangeStatusAsync_WhenInactive_Returns409AskingToReactivate()
    {
        Vehicle vehicle = _store.With(VehicleFactory.Create(_organizationId, deactivatedAt: FakeTimeProvider.ReferenceNow)).Vehicles[0];

        Response<Vehicle?> result = await _handler.ChangeStatusAsync(StatusRequest(vehicle.Id, EVehicleStatus.InMaintenance), CancellationToken.None);

        result.Code.Should().Be(409);
        result.Message.Should().Contain("Reative");
        _store.SaveCount.Should().Be(0);
    }

    [Fact]
    public async Task ChangeStatusAsync_ToTheCurrentStatus_ReturnsTheVehicleWithoutSaving()
    {
        Vehicle vehicle = _store.With(VehicleFactory.Create(_organizationId)).Vehicles[0];

        Response<Vehicle?> result = await _handler.ChangeStatusAsync(StatusRequest(vehicle.Id, EVehicleStatus.Available), CancellationToken.None);

        result.Code.Should().Be(200);
        _store.SaveCount.Should().Be(0);
    }

    [Fact]
    public async Task ChangeStatusAsync_OnAnotherOrganizationsVehicle_Returns404()
    {
        Vehicle foreign = _store.With(VehicleFactory.Create(_otherOrganizationId)).Vehicles[0];

        Response<Vehicle?> result = await _handler.ChangeStatusAsync(StatusRequest(foreign.Id, EVehicleStatus.InMaintenance), CancellationToken.None);

        result.Code.Should().Be(404);
        foreign.Status.Should().Be(EVehicleStatus.Available);
    }

    private ChangeVehicleStatusRequest StatusRequest(Guid vehicleId, EVehicleStatus status)
        => new() { Id = vehicleId, OrganizationId = _organizationId, Status = status };

    private CreateVehicleRequest CreateRequest(string plate)
        => new()
        {
            OrganizationId = _organizationId,
            Plate = plate,
            Model = " Volvo FH 540 ",
            CapacityKg = 27000,
            InitialOdometerKm = 120000,
            MaintenanceProvisionRatePerKm = 0.15m
        };

    private static ByIdRequest ById(Guid id, Guid organizationId) => new() { Id = id, OrganizationId = organizationId };
}
