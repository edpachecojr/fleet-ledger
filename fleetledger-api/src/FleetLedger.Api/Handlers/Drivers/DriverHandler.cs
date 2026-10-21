using FleetLedger.Api.Common.Time;
using FleetLedger.Api.Data.Stores;
using FleetLedger.Api.Data.Stores.Drivers;
using FleetLedger.Core.Handlers;
using FleetLedger.Core.Models;
using FleetLedger.Core.Requests;
using FleetLedger.Core.Requests.Drivers;
using FleetLedger.Core.Responses;
using FleetLedger.Core.Validators.Documents;

namespace FleetLedger.Api.Handlers.Drivers;

// Toda resposta leva a situação documental de hoje (DriverDocumentStatusRule).
public sealed class DriverHandler(
    IDriverStore store,
    TimeProvider timeProvider,
    IBusinessCalendar calendar,
    ILogger<DriverHandler> logger) : IDriverHandler
{
    public async Task<Response<Driver?>> CreateAsync(CreateDriverRequest request, CancellationToken cancellationToken)
    {
        string cpf = DigitsOnly.Of(request.Cpf);
        if (await store.CpfExistsAsync(request.OrganizationId, cpf, cancellationToken))
            return DriverResponses.DuplicateCpf(cpf);
        if (await CnhTakenAsync(request, ignoredDriverId: null, cancellationToken) is string takenCnh)
            return DriverResponses.DuplicateCnhNumber(takenCnh);

        DateTimeOffset now = timeProvider.GetUtcNow();
        Driver driver = new() { OrganizationId = request.OrganizationId, Cpf = cpf, CreatedAt = now, UpdatedAt = now };
        ApplyProfile(driver, request);
        store.Add(driver);
        await store.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Motorista {DriverId} cadastrado na organização {OrganizationId}", driver.Id, driver.OrganizationId);
        return DriverResponses.Created(WithDocumentStatus(driver));
    }

    public async Task<Response<Driver?>> UpdateAsync(UpdateDriverRequest request, CancellationToken cancellationToken)
    {
        Driver? driver = await store.FindAsync(request.OrganizationId, request.Id, cancellationToken);
        if (driver is null)
            return DriverResponses.NotFound(request.Id);
        if (await CnhTakenAsync(request, driver.Id, cancellationToken) is string takenCnh)
            return DriverResponses.DuplicateCnhNumber(takenCnh);

        ApplyProfile(driver, request);
        driver.UpdatedAt = timeProvider.GetUtcNow();
        await store.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Motorista {DriverId} atualizado", driver.Id);
        return DriverResponses.Updated(WithDocumentStatus(driver));
    }

    public async Task<Response<Driver?>> GetAsync(ByIdRequest request, CancellationToken cancellationToken)
    {
        Driver? driver = await store.FindAsync(request.OrganizationId, request.Id, cancellationToken);
        return driver is null ? DriverResponses.NotFound(request.Id) : DriverResponses.Found(WithDocumentStatus(driver));
    }

    public async Task<PagedResponse<List<Driver>>> ListAsync(ListDriversRequest request, CancellationToken cancellationToken)
    {
        DateOnly today = calendar.Today();
        StorePage<Driver> page = await store.ListAsync(request, today, cancellationToken);
        page.Items.ForEach(driver => driver.SetDocumentStatus(DriverDocumentStatusRule.Evaluate(driver, today)));

        return new PagedResponse<List<Driver>>(page.Items, page.TotalCount, request.EffectivePageNumber, request.EffectivePageSize);
    }

    public async Task<Response<Driver?>> DeactivateAsync(ByIdRequest request, CancellationToken cancellationToken)
        => await ChangeActivityAsync(request, deactivate: true, cancellationToken);

    public async Task<Response<Driver?>> ReactivateAsync(ByIdRequest request, CancellationToken cancellationToken)
        => await ChangeActivityAsync(request, deactivate: false, cancellationToken);

    private async Task<Response<Driver?>> ChangeActivityAsync(ByIdRequest request, bool deactivate, CancellationToken cancellationToken)
    {
        Driver? driver = await store.FindAsync(request.OrganizationId, request.Id, cancellationToken);
        if (driver is null)
            return DriverResponses.NotFound(request.Id);
        if (!ActivityChange.TryApply(driver, deactivate, timeProvider.GetUtcNow()))
            return DriverResponses.Found(WithDocumentStatus(driver));

        await store.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Motorista {DriverId} {DriverActivity}", driver.Id, deactivate ? "inativado" : "reativado");
        return DriverResponses.Found(WithDocumentStatus(driver));
    }

    // Devolve a CNH normalizada quando ela já pertence a outro motorista da organização.
    private async Task<string?> CnhTakenAsync(DriverProfileFields profile, Guid? ignoredDriverId, CancellationToken cancellationToken)
    {
        string cnhNumber = DigitsOnly.Of(profile.CnhNumber);
        bool taken = await store.CnhNumberExistsAsync(profile.OrganizationId, cnhNumber, ignoredDriverId, cancellationToken);
        return taken ? cnhNumber : null;
    }

    private Driver WithDocumentStatus(Driver driver)
    {
        driver.SetDocumentStatus(DriverDocumentStatusRule.Evaluate(driver, calendar.Today()));
        return driver;
    }

    private static void ApplyProfile(Driver driver, DriverProfileFields profile)
    {
        driver.Name = profile.Name.Trim();
        driver.Phone = DigitsOnly.Of(profile.Phone);
        driver.CnhNumber = DigitsOnly.Of(profile.CnhNumber);
        driver.CnhCategory = profile.CnhCategory;
        driver.CnhExpiresOn = profile.CnhExpiresOn;
        driver.ToxicologyExamExpiresOn = profile.ToxicologyExamExpiresOn;
        driver.DefaultCommissionPercent = profile.DefaultCommissionPercent;
        driver.BankDetails = OptionalText.TrimToNull(profile.BankDetails);
    }
}
