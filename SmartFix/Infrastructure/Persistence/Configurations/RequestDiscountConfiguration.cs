using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFix.Domain.Aggregates;

namespace SmartFix.Infrastructure.Persistence.Configurations;

public class RequestDiscountConfiguration : IEntityTypeConfiguration<RequestDiscount>
{
    public void Configure(EntityTypeBuilder<RequestDiscount> builder)
    {
        builder.HasKey(rd => rd.Id);
        builder.Property(rd => rd.SavedAmount).HasColumnType("decimal(18,2)");
        builder.HasOne(rd => rd.Discount)
            .WithMany()
            .HasForeignKey(rd => rd.DiscountId)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(rd=>rd.Request)
            .WithMany(r => r.AppliedDiscounts)
            .HasForeignKey(rd => rd.RequestId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}