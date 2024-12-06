using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryApp.Infrastructure.Postgres.EntityConfiguration;

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("id").ValueGeneratedNever();

        builder.OwnsOne(e => e.Location, c =>
        {
            c.Property(l => l.X).HasColumnName("location_x").IsRequired();
            c.Property(l => l.Y).HasColumnName("location_y").IsRequired();
        });

        builder.Property(e => e.CourierId).HasColumnName("courier_id").ValueGeneratedNever();

        builder.HasOne<Courier>()
            .WithMany()
            .IsRequired(false)
            .HasForeignKey(x => x.CourierId);

        builder.HasOne(e => e.Status)
            .WithMany()
            .IsRequired(true)
            .HasForeignKey("status_id");
    }
}
