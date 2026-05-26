using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WizCo.Orders.Application.Interfaces;
using WizCo.Orders.Infrastructure.Data;
using WizCo.Orders.Infrastructure.Repositories;

namespace WizCo.Orders.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' não configurada.");

        services.AddDbContext<OrdersDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<IPedidoRepository, PedidoRepository>();

        return services;
    }
}
