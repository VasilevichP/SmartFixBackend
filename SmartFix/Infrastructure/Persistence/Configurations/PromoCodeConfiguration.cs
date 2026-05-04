using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFix.Domain.Aggregates;

namespace SmartFix.Infrastructure.Persistence.Configurations;

public class PromoCodeConfiguration :IEntityTypeConfiguration<PromoCode>
{
    public void Configure(EntityTypeBuilder<PromoCode> builder)
    {
        builder.HasKey(p => p.Id);
        // builder.HasIndex(p => p.Code).IsUnique();
        builder.Property(p => p.Value).HasColumnType("decimal(18,2)");
        builder.Property(p => p.Code).HasMaxLength(30);
    }
}