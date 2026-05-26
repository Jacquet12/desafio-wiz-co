using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using WizCo.Orders.Application.DTOs.Requests;
using WizCo.Orders.Application.Interfaces;
using WizCo.Orders.Application.Mappings;
using WizCo.Orders.Application.Services;
using WizCo.Orders.Domain.Entities;
using WizCo.Orders.Domain.Enums;
using WizCo.Orders.Domain.Exceptions;

namespace WizCo.Orders.Tests.Services;

public class PedidoServiceTests
{
    private readonly Mock<IPedidoRepository> _repository = new();
    private readonly Mock<IValidator<CriarPedidoRequest>> _validator = new();
    private readonly IMapper _mapper;
    private readonly PedidoService _service;

    public PedidoServiceTests()
    {
        var mapperConfig = new MapperConfiguration(
            cfg => cfg.AddProfile<PedidoMappingProfile>(),
            NullLoggerFactory.Instance);

        _mapper = mapperConfig.CreateMapper();
        _service = new PedidoService(
            _repository.Object,
            _validator.Object,
            _mapper,
            NullLogger<PedidoService>.Instance);
    }

    [Fact]
    public async Task CriarAsync_Deve_Criar_Pedido_E_Chamar_Repositorio()
    {
        var request = new CriarPedidoRequest
        {
            ClienteNome = "Cliente",
            Itens =
            [
                new CriarItemPedidoRequest { ProdutoNome = "Item", Quantidade = 2, PrecoUnitario = 50 }
            ]
        };

        _validator
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var response = await _service.CriarAsync(request);

        response.ClienteNome.Should().Be("Cliente");
        response.ValorTotal.Should().Be(100);
        response.Status.Should().Be(StatusPedido.Novo.ToString());

        _repository.Verify(r => r.AdicionarAsync(It.IsAny<Pedido>(), It.IsAny<CancellationToken>()), Times.Once);
        _repository.Verify(r => r.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ObterPorIdAsync_Deve_Lancar_Excecao_Quando_Pedido_Nao_Existir()
    {
        var id = Guid.NewGuid();

        _repository
            .Setup(r => r.ObterPorIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Pedido?)null);

        var act = async () => await _service.ObterPorIdAsync(id);

        await act.Should().ThrowAsync<PedidoNaoEncontradoException>();
    }

    [Fact]
    public async Task CancelarAsync_Deve_Cancelar_Pedido_Existente()
    {
        var pedido = new Pedido("Cliente", [new ItemPedido("Item", 1, 10)]);

        _repository
            .Setup(r => r.ObterPorIdAsync(pedido.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pedido);

        await _service.CancelarAsync(pedido.Id);

        pedido.Status.Should().Be(StatusPedido.Cancelado);
        _repository.Verify(r => r.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ListarAsync_Deve_Rejeitar_Status_Invalido()
    {
        var act = async () => await _service.ListarAsync("Invalido");

        await act.Should().ThrowAsync<RegraNegocioException>()
            .WithMessage("*Status inválido*");
    }
}
