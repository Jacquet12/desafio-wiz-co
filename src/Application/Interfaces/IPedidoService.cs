using WizCo.Orders.Application.DTOs.Requests;
using WizCo.Orders.Application.DTOs.Responses;

namespace WizCo.Orders.Application.Interfaces;

public interface IPedidoService
{
    Task<PedidoResponse> CriarAsync(CriarPedidoRequest request, CancellationToken cancellationToken = default);
    Task<PedidoResponse> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResponse<PedidoResponse>> ListarAsync(
        string? status,
        int page = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);
    Task CancelarAsync(Guid id, CancellationToken cancellationToken = default);
}
