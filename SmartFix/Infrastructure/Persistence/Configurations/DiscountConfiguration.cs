using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFix.Domain.Aggregates;

namespace SmartFix.Infrastructure.Persistence.Configurations;

public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Value).HasColumnType("decimal(18,2)");

        builder.HasDiscriminator<string>("DiscountCategory")
            .HasValue<RequestsCountDiscount>("RequestsCount")
            .HasValue<DayOfWeekDiscount>("DayOfWeek")
            .HasValue<RequestSumDiscount>("RequestSum");
    }
}