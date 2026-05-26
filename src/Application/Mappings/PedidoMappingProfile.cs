using AutoMapper;
using WizCo.Orders.Application.DTOs.Responses;
using WizCo.Orders.Domain.Entities;

namespace WizCo.Orders.Application.Mappings;

public class PedidoMappingProfile : Profile
{
    public PedidoMappingProfile()
    {
        CreateMap<Pedido, PedidoResponse>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<ItemPedido, ItemPedidoResponse>()
            .ForMember(dest => dest.Subtotal, opt => opt.MapFrom(src => src.CalcularSubtotal()));
    }
}
