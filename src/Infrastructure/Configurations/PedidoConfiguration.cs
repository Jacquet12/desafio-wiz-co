using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WizCo.Orders.Domain.Entities;
using WizCo.Orders.Domain.Enums;

namespace WizCo.Orders.Infrastructure.Configurations;

public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {
        builder.ToTable("Pedidos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .IsRequired();

        builder.Property(p => p.ClienteNome)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.DataCriacao)
            .IsRequired();

        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion(
                status => status.ToString(),
                value => Enum.Parse<StatusPedido>(value))
            .HasMaxLength(20);

        builder.Property(p => p.ValorTotal)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.HasMany(p => p.Itens)
            .WithOne()
            .HasForeignKey(i => i.PedidoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(p => p.Status);
    }
}
