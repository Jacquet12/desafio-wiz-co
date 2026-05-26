namespace WizCo.Orders.Application.DTOs.Requests;

public class CriarPedidoRequest
{
    public string ClienteNome { get; set; } = string.Empty;
    public List<CriarItemPedidoRequest> Itens { get; set; } = [];
}
