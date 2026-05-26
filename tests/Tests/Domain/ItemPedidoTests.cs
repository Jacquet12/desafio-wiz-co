using FluentAssertions;
using WizCo.Orders.Domain.Entities;
using WizCo.Orders.Domain.Exceptions;

namespace WizCo.Orders.Tests.Domain;

public class ItemPedidoTests
{
    [Fact]
    public void Deve_Calcular_Subtotal_Corretamente()
    {
        var item = new ItemPedido("Produto", 3, 12.50m);

        item.CalcularSubtotal().Should().Be(37.50m);
    }

    [Fact]
    public void Deve_Lancar_Excecao_Quando_Quantidade_For_Invalida()
    {
        var act = () => new ItemPedido("Produto", 0, 10m);

        act.Should().Throw<DomainException>()
            .WithMessage("A quantidade deve ser maior que zero.");
    }
}
