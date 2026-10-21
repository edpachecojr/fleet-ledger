using FleetLedger.Core.Models;
using FleetLedger.Core.Responses;

namespace FleetLedger.Api.Handlers.Customers;

public static class CustomerResponses
{
    public static Response<Customer?> Created(Customer customer) => new(customer, StatusCodes.Status201Created);

    public static Response<Customer?> Found(Customer customer) => new(customer);

    public static Response<Customer?> Updated(Customer customer) => new(customer, message: "Cliente atualizado.");

    public static Response<Customer?> NotFound(Guid customerId)
        => new(null, StatusCodes.Status404NotFound, $"Cliente {customerId} não encontrado.");

    public static Response<Customer?> DuplicateCnpj(string cnpj)
        => new(null, StatusCodes.Status409Conflict, $"Já existe um cliente com o CNPJ {cnpj}.");
}
