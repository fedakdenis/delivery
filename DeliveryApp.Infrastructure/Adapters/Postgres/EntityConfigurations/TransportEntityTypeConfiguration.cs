using DeliveryApp.Core.Domain.Model.CourierAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryApp.Infrastructure.Postgres.EntityConfiguration;

public class TransportEntityTypeConfiguration : IEntityTypeConfiguration<Transport>
{
    public void Configure(EntityTypeBuilder<Transport> builder)
    {
        builder.ToTable("transport");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("id").ValueGeneratedNever();

        builder.Property(e => e.Name).HasColumnName("name");
        builder.HasIndex(e => e.Name);
        
        builder.Property(e => e.Speed).HasColumnName("speed").IsRequired();
        
        builder.HasData(Transport.List());
    }
}
