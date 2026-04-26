using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFix.Domain.Aggregates;

namespace SmartFix.Infrastructure.Persistence.Configurations;

public class RequestConfiguration:IEntityTypeConfiguration<Request>
{
    public void Configure(EntityTypeBuilder<Request> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.BasePrice).HasColumnType("decimal(18,2)");
        builder.Property(r => r.FinalPrice).HasColumnType("decimal(18,2)");
        builder.Property(r => r.DeviceModelName).IsRequired().HasMaxLength(150);

        builder.HasOne(r => r.Client)
            .WithMany()
            .HasForeignKey(r => r.ClientId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(r => r.Master)
            .WithMany()
            .HasForeignKey(r => r.MasterId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasOne<Request>()
            .WithMany()
            .HasForeignKey(r => r.ParentRequestId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(r => r.DeviceModel)
            .WithMany()
            .HasForeignKey(r => r.DeviceModelId)
            .IsRequired(false);
            
        builder.HasOne(r => r.DeviceType)
            .WithMany()
            .HasForeignKey(r => r.DeviceTypeId);
       
        builder.HasOne(r => r.PromoCode)
            .WithMany()
            .HasForeignKey(r => r.PromoCodeId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasMany(r => r.Photos)
            .WithOne()
            .HasForeignKey(p => p.RequestId);
    }
}