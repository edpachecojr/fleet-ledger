using FleetLedger.Api.Data.Stores;
using FleetLedger.Api.Data.Stores.Customers;
using FleetLedger.Core.Handlers;
using FleetLedger.Core.Models;
using FleetLedger.Core.Requests;
using FleetLedger.Core.Requests.Customers;
using FleetLedger.Core.Responses;
using FleetLedger.Core.Validators.Documents;
using static FleetLedger.Core.Validators.Customers.CustomerProfileRules;

namespace FleetLedger.Api.Handlers.Customers;

public sealed class CustomerHandler(
    ICustomerStore store,
    TimeProvider timeProvider,
    ILogger<CustomerHandler> logger) : ICustomerHandler
{
    public async Task<Response<Customer?>> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        string cnpj = DigitsOnly.Of(request.Cnpj);
        if (await store.CnpjExistsAsync(request.OrganizationId, cnpj, cancellationToken))
            return CustomerResponses.DuplicateCnpj(cnpj);

        DateTimeOffset now = timeProvider.GetUtcNow();
        Customer customer = new()
        {
            OrganizationId = request.OrganizationId,
            Cnpj = cnpj,
            CreatedAt = now,
            UpdatedAt = now
        };
        ApplyProfile(customer, new(request.CorporateName, request.TradeName, request.Phone, request.Email,
            request.BillingCycle, request.PaymentTermDays));
        store.Add(customer);
        await store.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Cliente {CustomerId} cadastrado na organização {OrganizationId}", customer.Id, customer.OrganizationId);
        return CustomerResponses.Created(customer);
    }

    public async Task<Response<Customer?>> UpdateAsync(UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        Customer? customer = await store.FindAsync(request.OrganizationId, request.Id, cancellationToken);
        if (customer is null)
            return CustomerResponses.NotFound(request.Id);

        ApplyProfile(customer, new(request.CorporateName, request.TradeName, request.Phone, request.Email,
            request.BillingCycle, request.PaymentTermDays));
        customer.UpdatedAt = timeProvider.GetUtcNow();
        await store.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Cliente {CustomerId} atualizado", customer.Id);
        return CustomerResponses.Updated(customer);
    }

    public async Task<Response<Customer?>> GetAsync(ByIdRequest request, CancellationToken cancellationToken)
    {
        Customer? customer = await store.FindAsync(request.OrganizationId, request.Id, cancellationToken);
        return customer is null ? CustomerResponses.NotFound(request.Id) : CustomerResponses.Found(customer);
    }

    public async Task<PagedResponse<List<Customer>>> ListAsync(ListCustomersRequest request, CancellationToken cancellationToken)
    {
        StorePage<Customer> page = await store.ListAsync(request, cancellationToken);
        return new PagedResponse<List<Customer>>(page.Items, page.TotalCount, request.EffectivePageNumber, request.EffectivePageSize);
    }

    public async Task<Response<Customer?>> DeactivateAsync(ByIdRequest request, CancellationToken cancellationToken)
        => await ChangeActivityAsync(request, deactivate: true, cancellationToken);

    public async Task<Response<Customer?>> ReactivateAsync(ByIdRequest request, CancellationToken cancellationToken)
        => await ChangeActivityAsync(request, deactivate: false, cancellationToken);

    private async Task<Response<Customer?>> ChangeActivityAsync(ByIdRequest request, bool deactivate, CancellationToken cancellationToken)
    {
        Customer? customer = await store.FindAsync(request.OrganizationId, request.Id, cancellationToken);
        if (customer is null)
            return CustomerResponses.NotFound(request.Id);
        if (!ActivityChange.TryApply(customer, deactivate, timeProvider.GetUtcNow()))
            return CustomerResponses.Found(customer);

        await store.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Cliente {CustomerId} {CustomerActivity}", customer.Id, deactivate ? "inativado" : "reativado");
        return CustomerResponses.Found(customer);
    }

    private static void ApplyProfile(Customer customer, CustomerProfile profile)
    {
        customer.CorporateName = profile.CorporateName?.Trim() ?? string.Empty;
        customer.TradeName = OptionalText.TrimToNull(profile.TradeName);
        customer.Phone = OptionalText.TrimToNull(DigitsOnly.Of(profile.Phone));
        customer.Email = OptionalText.TrimToNull(profile.Email);
        customer.BillingCycle = profile.BillingCycle;
        customer.PaymentTermDays = profile.PaymentTermDays;
    }
}
