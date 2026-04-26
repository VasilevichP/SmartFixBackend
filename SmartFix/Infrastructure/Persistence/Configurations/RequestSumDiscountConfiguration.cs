using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFix.Domain.Aggregates;

namespace SmartFix.Infrastructure.Persistence.Configurations;

public class RequestSumDiscountConfiguration:IEntityTypeConfiguration<RequestSumDiscount>
{
    public void Configure(EntityTypeBuilder<RequestSumDiscount> builder)
    {
        builder.Property(d => d.TargetSum).HasColumnType("decimal(18,2)");
    }
}