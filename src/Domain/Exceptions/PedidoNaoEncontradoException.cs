namespace WizCo.Orders.Domain.Exceptions;

public class PedidoNaoEncontradoException : DomainException
{
    public PedidoNaoEncontradoException(Guid pedidoId)
        : base($"Pedido com id '{pedidoId}' não foi encontrado.")
    {
    }
}
