using System.Text.Json.Serialization;

namespace FleetLedger.Core.Responses;

/// <summary>
/// Envelope único de retorno das operações. <see cref="Code"/> é o status HTTP calculado
/// pelo handler e devolvido tal qual pelo endpoint.
/// </summary>
/// <example><code>return new Response&lt;Vehicle?&gt;(null, 404, "Veículo não encontrado");</code></example>
/// <typeparam name="TData">Carga útil da resposta.</typeparam>
public class Response<TData>
{
    /// <summary>Construtor usado pela desserialização.</summary>
    public Response()
    {
    }

    /// <summary>Cria a resposta com dado, status HTTP e mensagem.</summary>
    public Response(TData? data, int code = 200, string? message = null)
    {
        Data = data;
        Code = code;
        Message = message;
    }

    /// <summary>Carga útil; nula em respostas de erro.</summary>
    public TData? Data { get; set; }

    /// <summary>Mensagem para o usuário. Como exibi-la é decisão do cliente.</summary>
    public string? Message { get; set; }

    /// <summary>
    /// Status HTTP da operação. Precisa ser público e serializado: o cliente depende dele
    /// e, sem ele, <see cref="IsSuccess"/> mente depois da desserialização (antipadrão A-02).
    /// </summary>
    public int Code { get; set; } = 200;

    /// <summary>Verdadeiro para status 2xx.</summary>
    [JsonIgnore]
    public bool IsSuccess => Code is >= 200 and <= 299;
}
