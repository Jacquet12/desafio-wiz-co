namespace WizCo.Orders.Domain.Exceptions;

public class RegraNegocioException : DomainException
{
    public RegraNegocioException(string message) : base(message)
    {
    }
}
