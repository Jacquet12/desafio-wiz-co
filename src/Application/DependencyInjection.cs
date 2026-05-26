using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using WizCo.Orders.Application.DTOs.Requests;
using WizCo.Orders.Application.Interfaces;
using WizCo.Orders.Application.Mappings;
using WizCo.Orders.Application.Services;
using WizCo.Orders.Application.Validators;

namespace WizCo.Orders.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IPedidoService, PedidoService>();
        services.AddScoped<IValidator<CriarPedidoRequest>, CriarPedidoRequestValidator>();
        services.AddScoped<IValidator<CriarItemPedidoRequest>, CriarItemPedidoRequestValidator>();

        var mapperConfiguration = new MapperConfiguration(
            cfg => cfg.AddProfile<PedidoMappingProfile>(),
            NullLoggerFactory.Instance);

        services.AddSingleton(mapperConfiguration);
        services.AddSingleton<IMapper>(sp => sp.GetRequiredService<MapperConfiguration>().CreateMapper());

        return services;
    }
}
