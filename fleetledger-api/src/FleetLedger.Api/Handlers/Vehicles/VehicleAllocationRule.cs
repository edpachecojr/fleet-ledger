using System.Linq.Expressions;
using FleetLedger.Core.Models;

namespace FleetLedger.Api.Handlers.Vehicles;

// "O veículo pode ser alocado numa viagem?" (TASK-005): ativo e Disponível. A mesma
// expressão filtra a consulta (SQL) e decide em memória, para as duas nunca divergirem.
public static class VehicleAllocationRule
{
    public static readonly Expression<Func<Vehicle, bool>> IsAllocatableExpression =
        vehicle => vehicle.DeactivatedAt == null && vehicle.Status == EVehicleStatus.Available;

    private static readonly Func<Vehicle, bool> IsAllocatableCompiled = IsAllocatableExpression.Compile();

    public static bool IsAllocatable(Vehicle vehicle) => IsAllocatableCompiled(vehicle);
}
