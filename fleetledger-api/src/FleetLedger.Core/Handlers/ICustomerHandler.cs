using FleetLedger.Core.Models;
using FleetLedger.Core.Requests;
using FleetLedger.Core.Requests.Customers;
using FleetLedger.Core.Responses;

namespace FleetLedger.Core.Handlers;

/// <summary>Cadastro de clientes da organização do usuário.</summary>
public interface ICustomerHandler
{
    /// <summary>Cadastra o cliente; 409 se o CNPJ já existe na organização.</summary>
    /// <example><code>Response&lt;Customer?&gt; result = await handler.CreateAsync(request, cancellationToken);</code></example>
    Task<Response<Customer?>> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken);

    /// <summary>Altera tudo menos o CNPJ; 404 se o cliente não é da organização.</summary>
    /// <example><code>await handler.UpdateAsync(request, cancellationToken);</code></example>
    Task<Response<Customer?>> UpdateAsync(UpdateCustomerRequest request, CancellationToken cancellationToken);

    /// <summary>Um cliente da organização; 404 se não existe nela.</summary>
    /// <example><code>await handler.GetAsync(new ByIdRequest { Id = id, OrganizationId = organizationId }, cancellationToken);</code></example>
    Task<Response<Customer?>> GetAsync(ByIdRequest request, CancellationToken cancellationToken);

    /// <summary>Página de clientes, ordenada pela razão social.</summary>
    /// <example><code>await handler.ListAsync(new ListCustomersRequest { Search = "agro" }, cancellationToken);</code></example>
    Task<PagedResponse<List<Customer>>> ListAsync(ListCustomersRequest request, CancellationToken cancellationToken);

    /// <summary>Inativa o cliente (some da abertura de viagem); repetir não muda nada.</summary>
    /// <example><code>await handler.DeactivateAsync(request, cancellationToken);</code></example>
    Task<Response<Customer?>> DeactivateAsync(ByIdRequest request, CancellationToken cancellationToken);

    /// <summary>Reativa o cliente; repetir não muda nada.</summary>
    /// <example><code>await handler.ReactivateAsync(request, cancellationToken);</code></example>
    Task<Response<Customer?>> ReactivateAsync(ByIdRequest request, CancellationToken cancellationToken);
}
