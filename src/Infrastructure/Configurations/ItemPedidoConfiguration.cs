using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WizCo.Orders.Domain.Entities;

namespace WizCo.Orders.Infrastructure.Configurations;

public class ItemPedidoConfiguration : IEntityTypeConfiguration<ItemPedido>
{
    public void Configure(EntityTypeBuilder<ItemPedido> builder)
    {
        builder.ToTable("ItensPedido");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .IsRequired();

        builder.Property(i => i.PedidoId)
            .IsRequired();

        builder.Property(i => i.ProdutoNome)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(i => i.Quantidade)
            .IsRequired();

        builder.Property(i => i.PrecoUnitario)
            .IsRequired()
            .HasPrecision(18, 2);
    }
}
