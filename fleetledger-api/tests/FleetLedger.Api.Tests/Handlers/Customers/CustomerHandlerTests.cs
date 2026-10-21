using FleetLedger.Api.Handlers.Customers;
using FleetLedger.Api.Tests.Fakes;
using FleetLedger.Api.Tests.Fixtures;
using FleetLedger.Core.Models;
using FleetLedger.Core.Requests;
using FleetLedger.Core.Requests.Customers;
using FleetLedger.Core.Responses;
using Microsoft.Extensions.Logging.Abstractions;

namespace FleetLedger.Api.Tests.Handlers.Customers;

public class CustomerHandlerTests
{
    private readonly Guid _organizationId = Guid.CreateVersion7();
    private readonly Guid _otherOrganizationId = Guid.CreateVersion7();
    private readonly FakeCustomerStore _store = new();
    private readonly CustomerHandler _handler;

    public CustomerHandlerTests()
        => _handler = new CustomerHandler(_store, new FakeTimeProvider(), NullLogger<CustomerHandler>.Instance);

    [Fact]
    public async Task CreateAsync_StoresDigitsOnlyAndBlankOptionalsAsNull()
    {
        Response<Customer?> result = await _handler.CreateAsync(CreateRequest("11.222.333/0001-81"), CancellationToken.None);

        result.Code.Should().Be(201);
        result.Data.Should().BeEquivalentTo(new
        {
            CorporateName = "Agro Sul Ltda",
            TradeName = (string?)null,
            Cnpj = "11222333000181",
            Phone = "11987654321",
            Email = (string?)null,
            BillingCycle = EBillingCycle.Monthly,
            IsActive = true,
            OrganizationId = _organizationId,
            CreatedAt = FakeTimeProvider.ReferenceNow
        });
    }

    [Fact]
    public async Task CreateAsync_WhenCnpjExistsInTheOrganization_Returns409WithTheCnpj()
    {
        _store.With(CustomerFactory.Create(_organizationId));

        Response<Customer?> result = await _handler.CreateAsync(CreateRequest("11222333000181"), CancellationToken.None);

        result.Code.Should().Be(409);
        result.Message.Should().Contain("11222333000181");
        _store.SaveCount.Should().Be(0);
    }

    [Fact]
    public async Task CreateAsync_WhenCnpjExistsOnlyInAnotherOrganization_Creates()
    {
        _store.With(CustomerFactory.Create(_otherOrganizationId));

        Response<Customer?> result = await _handler.CreateAsync(CreateRequest("11222333000181"), CancellationToken.None);

        result.Code.Should().Be(201);
    }

    [Fact]
    public async Task UpdateAsync_ChangesTheProfileAndKeepsTheCnpj()
    {
        Customer customer = _store.With(CustomerFactory.Create(_organizationId)).Customers[0];

        Response<Customer?> result = await _handler.UpdateAsync(
            new UpdateCustomerRequest
            {
                Id = customer.Id,
                OrganizationId = _organizationId,
                CorporateName = "Agro Sul Alimentos Ltda",
                Email = " financeiro@agrosul.com.br ",
                BillingCycle = EBillingCycle.Biweekly,
                PaymentTermDays = 15
            },
            CancellationToken.None);

        result.Data.Should().BeEquivalentTo(new
        {
            CorporateName = "Agro Sul Alimentos Ltda",
            Email = "financeiro@agrosul.com.br",
            Cnpj = "11222333000181",
            BillingCycle = EBillingCycle.Biweekly,
            PaymentTermDays = 15,
            UpdatedAt = FakeTimeProvider.ReferenceNow
        });
    }

    [Fact]
    public async Task Operations_OnAnotherOrganizationsCustomer_Return404AndChangeNothing()
    {
        Customer foreign = _store.With(CustomerFactory.Create(_otherOrganizationId)).Customers[0];

        Response<Customer?> get = await _handler.GetAsync(ById(foreign.Id), CancellationToken.None);
        Response<Customer?> update = await _handler.UpdateAsync(
            new UpdateCustomerRequest { Id = foreign.Id, OrganizationId = _organizationId, CorporateName = "Outro", BillingCycle = EBillingCycle.Weekly },
            CancellationToken.None);
        Response<Customer?> deactivate = await _handler.DeactivateAsync(ById(foreign.Id), CancellationToken.None);

        new[] { get.Code, update.Code, deactivate.Code }.Should().OnlyContain(code => code == 404);
        foreign.Should().BeEquivalentTo(new { CorporateName = "Agro Sul Ltda", IsActive = true });
    }

    [Fact]
    public async Task ListAsync_ReturnsOnlyTheOrganizationsCustomers()
    {
        _store.With(CustomerFactory.Create(_organizationId)).With(CustomerFactory.Create(_otherOrganizationId, "11444777000161"));

        PagedResponse<List<Customer>> result = await _handler.ListAsync(
            new ListCustomersRequest { OrganizationId = _organizationId },
            CancellationToken.None);

        result.Data!.Select(customer => customer.Cnpj).Should().Equal("11222333000181");
    }

    [Fact]
    public async Task DeactivateAndReactivate_ToggleTheActivity()
    {
        Customer customer = _store.With(CustomerFactory.Create(_organizationId)).Customers[0];

        Response<Customer?> deactivated = await _handler.DeactivateAsync(ById(customer.Id), CancellationToken.None);
        bool inactiveAfterDeactivate = deactivated.Data!.IsActive;
        Response<Customer?> reactivated = await _handler.ReactivateAsync(ById(customer.Id), CancellationToken.None);

        inactiveAfterDeactivate.Should().BeFalse();
        reactivated.Data!.IsActive.Should().BeTrue();
        _store.SaveCount.Should().Be(2);
    }

    private CreateCustomerRequest CreateRequest(string cnpj)
        => new()
        {
            OrganizationId = _organizationId,
            CorporateName = " Agro Sul Ltda ",
            TradeName = "  ",
            Cnpj = cnpj,
            Phone = "(11) 98765-4321",
            Email = "",
            BillingCycle = EBillingCycle.Monthly
        };

    private ByIdRequest ById(Guid id) => new() { Id = id, OrganizationId = _organizationId };
}
