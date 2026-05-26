using Microsoft.AspNetCore.Mvc;
using WizCo.Orders.Application.DTOs.Requests;
using WizCo.Orders.Application.Interfaces;

namespace WizCo.Orders.Api.Controllers;

[ApiController]
[Route("pedidos")]
public class PedidosController : ControllerBase
{
    private readonly IPedidoService _pedidoService;

    public PedidosController(IPedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    [HttpPost]
    public async Task<IActionResult> Criar(
        [FromBody] CriarPedidoRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _pedidoService.CriarAsync(request, cancellationToken);
        return CreatedAtAction(nameof(ObterPorId), new { id = response.Id }, response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id, CancellationToken cancellationToken)
    {
        var response = await _pedidoService.ObterPorIdAsync(id, cancellationToken);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> Listar(
        [FromQuery] string? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var response = await _pedidoService.ListarAsync(status, page, pageSize, cancellationToken);
        return Ok(response);
    }

    [HttpPut("{id:guid}/cancelar")]
    public async Task<IActionResult> Cancelar(Guid id, CancellationToken cancellationToken)
    {
        await _pedidoService.CancelarAsync(id, cancellationToken);
        return NoContent();
    }
}
