using FluentAssertions;
using WizCo.Orders.Domain.Entities;
using WizCo.Orders.Domain.Enums;
using WizCo.Orders.Domain.Exceptions;

namespace WizCo.Orders.Tests.Domain;

public class PedidoTests
{
    [Fact]
    public void Deve_Calcular_ValorTotal_Ao_Criar_Pedido()
    {
        var itens = new List<ItemPedido>
        {
            new("Produto A", 2, 10m),
            new("Produto B", 1, 5.50m)
        };

        var pedido = new Pedido("Cliente Teste", itens);

        pedido.ValorTotal.Should().Be(25.50m);
        pedido.Status.Should().Be(StatusPedido.Novo);
    }

    [Fact]
    public void Deve_Cancelar_Pedido_Novo_Com_Sucesso()
    {
        var pedido = CriarPedidoValido();

        pedido.Cancelar();

        pedido.Status.Should().Be(StatusPedido.Cancelado);
    }

    [Fact]
    public void Deve_Lancar_Excecao_Quando_Pedido_Pago_For_Cancelado()
    {
        var pedido = CriarPedidoValido();
        pedido.MarcarComoPago();

        var act = () => pedido.Cancelar();

        act.Should().Throw<RegraNegocioException>()
            .WithMessage("Pedido pago não pode ser cancelado.");
    }

    [Fact]
    public void Deve_Lancar_Excecao_Quando_Pedido_Sem_Itens()
    {
        var act = () => new Pedido("Cliente", []);

        act.Should().Throw<DomainException>()
            .WithMessage("O pedido deve conter pelo menos um item.");
    }

    private static Pedido CriarPedidoValido()
    {
        return new Pedido("Cliente", [new ItemPedido("Produto", 1, 100m)]);
    }
}
