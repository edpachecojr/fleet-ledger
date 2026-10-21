using FleetLedger.Core.Models;
using FleetLedger.Core.Requests;
using FleetLedger.Core.Requests.Vehicles;
using FleetLedger.Core.Responses;

namespace FleetLedger.Core.Handlers;

/// <summary>Cadastro de veículos da organização do usuário.</summary>
public interface IVehicleHandler
{
    /// <summary>Cadastra o veículo como Disponível; 409 se a placa já existe na organização.</summary>
    /// <example><code>Response&lt;Vehicle?&gt; result = await handler.CreateAsync(request, cancellationToken);</code></example>
    Task<Response<Vehicle?>> CreateAsync(CreateVehicleRequest request, CancellationToken cancellationToken);

    /// <summary>Altera modelo, capacidade e taxa; 404 se o veículo não é da organização.</summary>
    /// <example><code>await handler.UpdateAsync(request, cancellationToken);</code></example>
    Task<Response<Vehicle?>> UpdateAsync(UpdateVehicleRequest request, CancellationToken cancellationToken);

    /// <summary>Um veículo da organização; 404 se não existe nela.</summary>
    /// <example><code>await handler.GetAsync(new ByIdRequest { Id = id, OrganizationId = organizationId }, cancellationToken);</code></example>
    Task<Response<Vehicle?>> GetAsync(ByIdRequest request, CancellationToken cancellationToken);

    /// <summary>Página de veículos, ordenada pela placa.</summary>
    /// <example><code>await handler.ListAsync(new ListVehiclesRequest { Search = "volvo" }, cancellationToken);</code></example>
    Task<PagedResponse<List<Vehicle>>> ListAsync(ListVehiclesRequest request, CancellationToken cancellationToken);

    /// <summary>Inativa o veículo (em qualquer status); repetir não muda nada.</summary>
    /// <example><code>await handler.DeactivateAsync(request, cancellationToken);</code></example>
    Task<Response<Vehicle?>> DeactivateAsync(ByIdRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Troca manual entre Disponível e Em manutenção. 409 se o veículo está em viagem ou inativo;
    /// pedir o status que ele já tem não muda nada.
    /// </summary>
    /// <example><code>await handler.ChangeStatusAsync(new ChangeVehicleStatusRequest { Id = id, Status = EVehicleStatus.InMaintenance }, cancellationToken);</code></example>
    Task<Response<Vehicle?>> ChangeStatusAsync(ChangeVehicleStatusRequest request, CancellationToken cancellationToken);

    /// <summary>Reativa o veículo, mantendo o status operacional; repetir não muda nada.</summary>
    /// <example><code>await handler.ReactivateAsync(request, cancellationToken);</code></example>
    Task<Response<Vehicle?>> ReactivateAsync(ByIdRequest request, CancellationToken cancellationToken);
}
