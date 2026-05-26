using WizCo.Orders.Domain.Exceptions;

namespace WizCo.Orders.Domain.Entities;

public class ItemPedido
{
    public Guid Id { get; private set; }
    public Guid PedidoId { get; internal set; }
    public string ProdutoNome { get; private set; } = string.Empty;
    public int Quantidade { get; private set; }
    public decimal PrecoUnitario { get; private set; }

    protected ItemPedido()
    {
    }

    public ItemPedido(string produtoNome, int quantidade, decimal precoUnitario)
    {
        Validar(produtoNome, quantidade, precoUnitario);

        Id = Guid.NewGuid();
        ProdutoNome = produtoNome.Trim();
        Quantidade = quantidade;
        PrecoUnitario = precoUnitario;
    }

    public decimal CalcularSubtotal() => Quantidade * PrecoUnitario;

    internal static void Validar(string produtoNome, int quantidade, decimal precoUnitario)
    {
        if (string.IsNullOrWhiteSpace(produtoNome))
            throw new DomainException("O nome do produto é obrigatório.");

        if (quantidade <= 0)
            throw new DomainException("A quantidade deve ser maior que zero.");

        if (precoUnitario <= 0)
            throw new DomainException("O preço unitário deve ser maior que zero.");
    }
}
