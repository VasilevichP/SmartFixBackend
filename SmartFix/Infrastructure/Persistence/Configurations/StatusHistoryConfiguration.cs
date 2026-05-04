using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFix.Domain.Aggregates;

namespace SmartFix.Infrastructure.Persistence.Configurations;

public class StatusHistoryConfiguration:IEntityTypeConfiguration<StatusHistory>
{
    public void Configure(EntityTypeBuilder<StatusHistory> builder)
    {
        builder.HasKey(h => h.Id);
        builder.Property(h => h.Status)
            .HasConversion<string>()
            .HasMaxLength(50);
        builder.HasOne(h => h.Request)
            .WithMany()
            .HasForeignKey(h => h.RequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}