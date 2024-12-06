using DeliveryApp.Core.Domain.Model.CourierAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryApp.Infrastructure.Postgres.EntityConfiguration;

public class CourierStatusEntityTypeConfiguration : IEntityTypeConfiguration<CourierStatus>
{
    public void Configure(EntityTypeBuilder<CourierStatus> builder)
    {
        builder.ToTable("courier_status");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("id").ValueGeneratedNever();

        builder.Property(e => e.Name).HasColumnName("name");
        builder.HasIndex(e => e.Name);
        
        builder.HasData(CourierStatus.List());
    }
}
