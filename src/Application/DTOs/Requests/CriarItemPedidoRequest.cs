namespace WizCo.Orders.Application.DTOs.Requests;

public class CriarItemPedidoRequest
{
    public string ProdutoNome { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
}
