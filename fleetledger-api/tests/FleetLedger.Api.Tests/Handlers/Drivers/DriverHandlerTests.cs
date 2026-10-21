using FleetLedger.Api.Handlers.Drivers;
using FleetLedger.Api.Tests.Fakes;
using FleetLedger.Api.Tests.Fixtures;
using FleetLedger.Core.Models;
using FleetLedger.Core.Requests;
using FleetLedger.Core.Requests.Drivers;
using FleetLedger.Core.Responses;
using Microsoft.Extensions.Logging.Abstractions;

namespace FleetLedger.Api.Tests.Handlers.Drivers;

public class DriverHandlerTests
{
    private readonly Guid _organizationId = Guid.CreateVersion7();
    private readonly Guid _otherOrganizationId = Guid.CreateVersion7();
    private readonly FakeDriverStore _store = new();
    private readonly DriverHandler _handler;

    public DriverHandlerTests()
        => _handler = new DriverHandler(_store, new FakeTimeProvider(), new FakeBusinessCalendar(), NullLogger<DriverHandler>.Instance);

    [Fact]
    public async Task CreateAsync_StoresDigitsOnlyAndReturnsTodaysDocumentStatus()
    {
        CreateDriverRequest request = CreateRequest();
        request.CnhExpiresOn = FakeBusinessCalendar.ReferenceToday.AddDays(10);

        Response<Driver?> result = await _handler.CreateAsync(request, CancellationToken.None);

        result.Code.Should().Be(201);
        result.Data.Should().BeEquivalentTo(new
        {
            Cpf = "52998224725",
            Phone = "11987654321",
            CnhNumber = "12345678901",
            BankDetails = (string?)null,
            DocumentStatus = EDocumentStatus.ExpiringSoon,
            OrganizationId = _organizationId,
            CreatedAt = FakeTimeProvider.ReferenceNow
        });
    }

    [Fact]
    public async Task CreateAsync_WhenCpfExistsInTheOrganization_Returns409WithTheCpf()
    {
        _store.With(DriverFactory.Create(_organizationId, cnhNumber: "99999999999"));

        Response<Driver?> result = await _handler.CreateAsync(CreateRequest(), CancellationToken.None);

        result.Code.Should().Be(409);
        result.Message.Should().Contain("52998224725");
    }

    [Fact]
    public async Task CreateAsync_WhenCnhNumberExistsInTheOrganization_Returns409WithTheCnh()
    {
        _store.With(DriverFactory.Create(_organizationId, cpf: "11144477735"));

        Response<Driver?> result = await _handler.CreateAsync(CreateRequest(), CancellationToken.None);

        result.Code.Should().Be(409);
        result.Message.Should().Contain("12345678901");
        _store.SaveCount.Should().Be(0);
    }

    [Fact]
    public async Task CreateAsync_WhenDocumentsExistOnlyInAnotherOrganization_Creates()
    {
        _store.With(DriverFactory.Create(_otherOrganizationId));

        Response<Driver?> result = await _handler.CreateAsync(CreateRequest(), CancellationToken.None);

        result.Code.Should().Be(201);
    }

    [Fact]
    public async Task UpdateAsync_KeepingItsOwnCnh_SavesAndKeepsTheCpf()
    {
        Driver driver = _store.With(DriverFactory.Create(_organizationId)).Drivers[0];

        Response<Driver?> result = await _handler.UpdateAsync(UpdateRequest(driver.Id, "12345678901"), CancellationToken.None);

        result.Data.Should().BeEquivalentTo(new { Name = "João Pereira", Cpf = "52998224725", DefaultCommissionPercent = 15m });
    }

    [Fact]
    public async Task UpdateAsync_ToACnhOfAnotherDriver_Returns409()
    {
        Driver driver = _store.With(DriverFactory.Create(_organizationId))
            .With(DriverFactory.Create(_organizationId, cpf: "11144477735", cnhNumber: "99999999999")).Drivers[0];

        Response<Driver?> result = await _handler.UpdateAsync(UpdateRequest(driver.Id, "99999999999"), CancellationToken.None);

        result.Code.Should().Be(409);
        driver.Name.Should().Be("João da Silva");
    }

    [Fact]
    public async Task Operations_OnAnotherOrganizationsDriver_Return404AndChangeNothing()
    {
        Driver foreign = _store.With(DriverFactory.Create(_otherOrganizationId)).Drivers[0];

        Response<Driver?> get = await _handler.GetAsync(ById(foreign.Id), CancellationToken.None);
        Response<Driver?> update = await _handler.UpdateAsync(UpdateRequest(foreign.Id, "12345678901"), CancellationToken.None);
        Response<Driver?> deactivate = await _handler.DeactivateAsync(ById(foreign.Id), CancellationToken.None);

        new[] { get.Code, update.Code, deactivate.Code }.Should().OnlyContain(code => code == 404);
        foreign.Should().BeEquivalentTo(new { Name = "João da Silva", IsActive = true });
    }

    [Fact]
    public async Task ListAsync_ReturnsOnlyTheOrganizationsDriversWithTodaysStatus()
    {
        _store.With(DriverFactory.Create(_organizationId, cnhExpiresOn: FakeBusinessCalendar.ReferenceToday.AddDays(-1)))
            .With(DriverFactory.Create(_otherOrganizationId));

        PagedResponse<List<Driver>> result = await _handler.ListAsync(new ListDriversRequest { OrganizationId = _organizationId }, CancellationToken.None);

        result.Data.Should().ContainSingle().Which.DocumentStatus.Should().Be(EDocumentStatus.Expired);
        _store.LastListToday.Should().Be(FakeBusinessCalendar.ReferenceToday);
    }

    [Fact]
    public async Task DeactivateAndReactivate_ToggleTheActivity()
    {
        Driver driver = _store.With(DriverFactory.Create(_organizationId)).Drivers[0];

        bool activeAfterDeactivate = (await _handler.DeactivateAsync(ById(driver.Id), CancellationToken.None)).Data!.IsActive;
        bool activeAfterReactivate = (await _handler.ReactivateAsync(ById(driver.Id), CancellationToken.None)).Data!.IsActive;

        activeAfterDeactivate.Should().BeFalse();
        activeAfterReactivate.Should().BeTrue();
    }

    private CreateDriverRequest CreateRequest()
        => new()
        {
            OrganizationId = _organizationId,
            Name = " João da Silva ",
            Cpf = "529.982.247-25",
            Phone = "(11) 98765-4321",
            CnhNumber = "123.456.789-01",
            CnhCategory = ECnhCategory.E,
            CnhExpiresOn = FakeBusinessCalendar.ReferenceToday.AddYears(2),
            DefaultCommissionPercent = 12.5m,
            BankDetails = " "
        };

    private UpdateDriverRequest UpdateRequest(Guid driverId, string cnhNumber)
        => new()
        {
            Id = driverId,
            OrganizationId = _organizationId,
            Name = "João Pereira",
            Phone = "11987654321",
            CnhNumber = cnhNumber,
            CnhCategory = ECnhCategory.E,
            CnhExpiresOn = FakeBusinessCalendar.ReferenceToday.AddYears(2),
            DefaultCommissionPercent = 15m
        };

    private ByIdRequest ById(Guid id) => new() { Id = id, OrganizationId = _organizationId };
}
