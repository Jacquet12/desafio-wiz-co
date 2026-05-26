namespace WizCo.Orders.Application.DTOs.Responses;

public class ItemPedidoResponse
{
    public Guid Id { get; set; }
    public string ProdutoNome { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Subtotal { get; set; }
}
