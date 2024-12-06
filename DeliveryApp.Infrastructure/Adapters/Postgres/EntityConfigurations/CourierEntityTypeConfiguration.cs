using DeliveryApp.Core.Domain.Model.CourierAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryApp.Infrastructure.Postgres.EntityConfiguration;

public class CourierEntityTypeConfiguration : IEntityTypeConfiguration<Courier>
{
    public void Configure(EntityTypeBuilder<Courier> builder)
    {
        builder.ToTable("couriers");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("id").ValueGeneratedNever();

        builder.Property(e => e.Name).HasColumnName("name").IsRequired();

        builder.OwnsOne(e => e.Location, c =>
        {
            c.Property(l => l.X).HasColumnName("location_x").IsRequired();
            c.Property(l => l.Y).HasColumnName("location_y").IsRequired();
        });

        builder.HasOne(e => e.Transport)
            .WithMany()
            .IsRequired(true)
            .HasForeignKey("transport_id");

        builder.HasOne(e => e.Status)
            .WithMany()
            .IsRequired(true)
            .HasForeignKey("status_id");
    }
}