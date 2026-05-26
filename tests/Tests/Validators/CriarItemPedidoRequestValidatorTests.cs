using FluentValidation.TestHelper;
using WizCo.Orders.Application.DTOs.Requests;
using WizCo.Orders.Application.Validators;

namespace WizCo.Orders.Tests.Validators;

public class CriarItemPedidoRequestValidatorTests
{
    private readonly CriarItemPedidoRequestValidator _validator = new();

    [Fact]
    public void Deve_Ser_Invalido_Quando_Quantidade_For_Menor_Ou_Igual_A_Zero()
    {
        var request = new CriarItemPedidoRequest
        {
            ProdutoNome = "Produto",
            Quantidade = 0,
            PrecoUnitario = 10
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Quantidade);
    }

    [Fact]
    public void Deve_Ser_Invalido_Quando_PrecoUnitario_For_Menor_Ou_Igual_A_Zero()
    {
        var request = new CriarItemPedidoRequest
        {
            ProdutoNome = "Produto",
            Quantidade = 1,
            PrecoUnitario = 0
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.PrecoUnitario);
    }
}
