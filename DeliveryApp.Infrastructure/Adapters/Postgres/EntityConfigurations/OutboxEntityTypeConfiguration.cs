using DeliveryApp.Infrastructure.Adapters.Postgres.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryApp.Infrastructure.Postgres.EntityConfiguration;

internal class OutboxEntityTypeConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder
            .ToTable("outbox");

        builder
            .Property(entity => entity.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");

        builder
            .Property(entity => entity.Type)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("type")
            .IsRequired();

        builder
            .Property(entity => entity.Content)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("content")
            .IsRequired();

        builder
            .Property(entity => entity.OccurredOnUtc)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("occurred_on_utc")
            .IsRequired();

        builder
            .Property(entity => entity.ProcessedOnUtc)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("processed_on_utc")
            .IsRequired(false);
    }
}
