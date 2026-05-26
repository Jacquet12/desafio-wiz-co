using FluentValidation;
using WizCo.Orders.Application.DTOs.Requests;

namespace WizCo.Orders.Application.Validators;

public class CriarPedidoRequestValidator : AbstractValidator<CriarPedidoRequest>
{
    public CriarPedidoRequestValidator()
    {
        RuleFor(x => x.ClienteNome)
            .NotEmpty().WithMessage("O nome do cliente é obrigatório.")
            .MaximumLength(200).WithMessage("O nome do cliente deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Itens)
            .NotNull().WithMessage("A lista de itens é obrigatória.")
            .NotEmpty().WithMessage("O pedido deve conter pelo menos um item.");

        RuleForEach(x => x.Itens)
            .SetValidator(new CriarItemPedidoRequestValidator());
    }
}
