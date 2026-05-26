using WizCo.Orders.Domain.Enums;
using WizCo.Orders.Domain.Exceptions;

namespace WizCo.Orders.Domain.Entities;

public class Pedido
{
    public Guid Id { get; private set; }
    public string ClienteNome { get; private set; } = string.Empty;
    public DateTime DataCriacao { get; private set; }
    public StatusPedido Status { get; private set; }
    public decimal ValorTotal { get; private set; }
    public ICollection<ItemPedido> Itens { get; private set; } = new List<ItemPedido>();

    protected Pedido()
    {
    }

    public Pedido(string clienteNome, IEnumerable<ItemPedido> itens)
    {
        if (string.IsNullOrWhiteSpace(clienteNome))
            throw new DomainException("O nome do cliente é obrigatório.");

        var listaItens = itens?.ToList() ?? [];
        if (listaItens.Count == 0)
            throw new DomainException("O pedido deve conter pelo menos um item.");

        Id = Guid.NewGuid();
        ClienteNome = clienteNome.Trim();
        DataCriacao = DateTime.UtcNow;
        Status = StatusPedido.Novo;
        Itens = listaItens;

        foreach (var item in Itens)
            item.PedidoId = Id;

        RecalcularValorTotal();
    }

    public void MarcarComoPago()
    {
        if (Status == StatusPedido.Cancelado)
            throw new RegraNegocioException("Pedido cancelado não pode ser marcado como pago.");

        if (Status == StatusPedido.Pago)
            return;

        Status = StatusPedido.Pago;
    }

    public void Cancelar()
    {
        if (Status == StatusPedido.Pago)
            throw new RegraNegocioException("Pedido pago não pode ser cancelado.");

        if (Status == StatusPedido.Cancelado)
            return;

        Status = StatusPedido.Cancelado;
    }

    public void RecalcularValorTotal()
    {
        ValorTotal = Itens.Sum(item => item.CalcularSubtotal());
    }
}
