using FleetLedger.Core.Models;
using FleetLedger.Core.Requests;
using FleetLedger.Core.Requests.Drivers;
using FleetLedger.Core.Responses;

namespace FleetLedger.Core.Handlers;

/// <summary>Cadastro de motoristas da organização do usuário, com a situação documental de hoje.</summary>
public interface IDriverHandler
{
    /// <summary>Cadastra o motorista; 409 se o CPF ou o número da CNH já existe na organização.</summary>
    /// <example><code>Response&lt;Driver?&gt; result = await handler.CreateAsync(request, cancellationToken);</code></example>
    Task<Response<Driver?>> CreateAsync(CreateDriverRequest request, CancellationToken cancellationToken);

    /// <summary>Altera tudo menos o CPF; 404 se não é da organização, 409 se a CNH é de outro motorista.</summary>
    /// <example><code>await handler.UpdateAsync(request, cancellationToken);</code></example>
    Task<Response<Driver?>> UpdateAsync(UpdateDriverRequest request, CancellationToken cancellationToken);

    /// <summary>Um motorista da organização; 404 se não existe nela.</summary>
    /// <example><code>await handler.GetAsync(new ByIdRequest { Id = id, OrganizationId = organizationId }, cancellationToken);</code></example>
    Task<Response<Driver?>> GetAsync(ByIdRequest request, CancellationToken cancellationToken);

    /// <summary>Página de motoristas, ordenada pelo nome.</summary>
    /// <example><code>await handler.ListAsync(new ListDriversRequest { DocumentStatus = EDocumentStatus.Expired }, cancellationToken);</code></example>
    Task<PagedResponse<List<Driver>>> ListAsync(ListDriversRequest request, CancellationToken cancellationToken);

    /// <summary>Inativa o motorista (some da abertura de viagem); repetir não muda nada.</summary>
    /// <example><code>await handler.DeactivateAsync(request, cancellationToken);</code></example>
    Task<Response<Driver?>> DeactivateAsync(ByIdRequest request, CancellationToken cancellationToken);

    /// <summary>Reativa o motorista; repetir não muda nada.</summary>
    /// <example><code>await handler.ReactivateAsync(request, cancellationToken);</code></example>
    Task<Response<Driver?>> ReactivateAsync(ByIdRequest request, CancellationToken cancellationToken);
}
