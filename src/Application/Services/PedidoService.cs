using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using WizCo.Orders.Application.DTOs.Requests;
using WizCo.Orders.Application.DTOs.Responses;
using WizCo.Orders.Application.Interfaces;
using WizCo.Orders.Domain.Entities;
using WizCo.Orders.Domain.Enums;
using WizCo.Orders.Domain.Exceptions;

namespace WizCo.Orders.Application.Services;

public class PedidoService : IPedidoService
{
    private readonly IPedidoRepository _repository;
    private readonly IValidator<CriarPedidoRequest> _criarPedidoValidator;
    private readonly IMapper _mapper;
    private readonly ILogger<PedidoService> _logger;

    public PedidoService(
        IPedidoRepository repository,
        IValidator<CriarPedidoRequest> criarPedidoValidator,
        IMapper mapper,
        ILogger<PedidoService> logger)
    {
        _repository = repository;
        _criarPedidoValidator = criarPedidoValidator;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PedidoResponse> CriarAsync(
        CriarPedidoRequest request,
        CancellationToken cancellationToken = default)
    {
        await ValidarRequestAsync(request, cancellationToken);

        var itens = request.Itens
            .Select(i => new ItemPedido(i.ProdutoNome, i.Quantidade, i.PrecoUnitario))
            .ToList();

        var pedido = new Pedido(request.ClienteNome, itens);

        await _repository.AdicionarAsync(pedido, cancellationToken);
        await _repository.SalvarAlteracoesAsync(cancellationToken);

        _logger.LogInformation("Pedido {PedidoId} criado", pedido.Id);

        return _mapper.Map<PedidoResponse>(pedido);
    }

    public async Task<PedidoResponse> ObterPorIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var pedido = await _repository.ObterPorIdAsync(id, cancellationToken)
            ?? throw new PedidoNaoEncontradoException(id);

        return _mapper.Map<PedidoResponse>(pedido);
    }

    public async Task<PagedResponse<PedidoResponse>> ListarAsync(
        string? status,
        int page = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var statusFiltro = ParseStatus(status);
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 1 : Math.Min(pageSize, 50);

        var (items, total) = await _repository.ListarAsync(
            statusFiltro,
            page,
            pageSize,
            cancellationToken);

        var totalPages = pageSize > 0
            ? (int)Math.Ceiling(total / (double)pageSize)
            : 0;

        return new PagedResponse<PedidoResponse>
        {
            Items = _mapper.Map<List<PedidoResponse>>(items),
            Page = page,
            PageSize = pageSize,
            TotalItems = total,
            TotalPages = totalPages
        };
    }

    public async Task CancelarAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var pedido = await _repository.ObterPorIdAsync(id, cancellationToken)
            ?? throw new PedidoNaoEncontradoException(id);

        pedido.Cancelar();
        await _repository.SalvarAlteracoesAsync(cancellationToken);

        _logger.LogInformation("Pedido {PedidoId} cancelado", id);
    }

    private async Task ValidarRequestAsync(
        CriarPedidoRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _criarPedidoValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
    }

    private static StatusPedido? ParseStatus(string? status)
    {
        if (string.IsNullOrWhiteSpace(status))
            return null;

        if (!Enum.TryParse<StatusPedido>(status, ignoreCase: true, out var parsed))
            throw new RegraNegocioException(
                $"Status inválido: '{status}'. Valores aceitos: Novo, Pago, Cancelado.");

        return parsed;
    }
}
