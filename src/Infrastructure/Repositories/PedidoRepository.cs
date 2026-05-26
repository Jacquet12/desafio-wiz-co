using Microsoft.EntityFrameworkCore;
using WizCo.Orders.Application.Interfaces;
using WizCo.Orders.Domain.Entities;
using WizCo.Orders.Domain.Enums;
using WizCo.Orders.Infrastructure.Data;

namespace WizCo.Orders.Infrastructure.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly OrdersDbContext _context;

    public PedidoRepository(OrdersDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(Pedido pedido, CancellationToken cancellationToken = default)
    {
        await _context.Pedidos.AddAsync(pedido, cancellationToken);
    }

    public async Task<Pedido?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Pedidos
            .Include(p => p.Itens)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<(IReadOnlyList<Pedido> Items, int Total)> ListarAsync(
        StatusPedido? status,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Pedidos
            .AsNoTracking()
            .Include(p => p.Itens)
            .AsQueryable();

        if (status.HasValue)
            query = query.Where(p => p.Status == status.Value);

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(p => p.DataCriacao)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, total);
    }

    public Task SalvarAlteracoesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
