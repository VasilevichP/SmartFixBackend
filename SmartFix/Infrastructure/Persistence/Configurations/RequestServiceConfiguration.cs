using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFix.Domain.Aggregates;

namespace SmartFix.Infrastructure.Persistence.Configurations;

public class RequestServiceConfiguration:IEntityTypeConfiguration<RequestService>
{
    public void Configure(EntityTypeBuilder<RequestService> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Price).HasColumnType("decimal(18,2)");
            
        builder.HasOne(r => r.Request)
            .WithMany(r=>r.Services)
            .HasForeignKey(r => r.RequestId)
            .OnDelete(DeleteBehavior.Cascade); 
            
        builder.HasOne(r => r.Service)
            .WithMany()
            .HasForeignKey(r => r.ServiceId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}