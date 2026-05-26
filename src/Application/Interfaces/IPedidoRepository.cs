using WizCo.Orders.Domain.Entities;
using WizCo.Orders.Domain.Enums;

namespace WizCo.Orders.Application.Interfaces;

public interface IPedidoRepository
{
    Task AdicionarAsync(Pedido pedido, CancellationToken cancellationToken = default);
    Task<Pedido?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Pedido> Items, int Total)> ListarAsync(
        StatusPedido? status,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
    Task SalvarAlteracoesAsync(CancellationToken cancellationToken = default);
}
