using FluentValidation.TestHelper;
using WizCo.Orders.Application.DTOs.Requests;
using WizCo.Orders.Application.Validators;

namespace WizCo.Orders.Tests.Validators;

public class CriarPedidoRequestValidatorTests
{
    private readonly CriarPedidoRequestValidator _validator = new();

    [Fact]
    public void Deve_Ser_Invalido_Quando_ClienteNome_Estiver_Vazio()
    {
        var request = new CriarPedidoRequest
        {
            ClienteNome = "",
            Itens = [new CriarItemPedidoRequest { ProdutoNome = "X", Quantidade = 1, PrecoUnitario = 10 }]
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.ClienteNome);
    }

    [Fact]
    public void Deve_Ser_Invalido_Quando_Itens_Estiver_Vazio()
    {
        var request = new CriarPedidoRequest
        {
            ClienteNome = "Cliente",
            Itens = []
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Itens);
    }
}
