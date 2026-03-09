using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFix.Domain.Aggregates;

namespace SmartFix.Infrastructure.Persistence.Configurations;

public class RequestConfiguration:IEntityTypeConfiguration<Request>
{
    public void Configure(EntityTypeBuilder<Request> builder)
    {
        builder.HasIndex(r => r.SpecialistId);
        builder.HasIndex(r => r.ClientId);
            
        builder.Property(r => r.DeviceModelName).IsRequired().HasMaxLength(150);

        builder.HasOne(r => r.DeviceModel)
            .WithMany()
            .HasForeignKey(r => r.DeviceModelId)
            .IsRequired(false);
            
        builder.HasOne(r => r.DeviceType)
            .WithMany()
            .HasForeignKey(r => r.DeviceTypeId);
        builder.HasOne(r => r.Specialist)
            .WithMany()
            .HasForeignKey(r => r.SpecialistId)
            .IsRequired(false); 
        builder.HasMany(r => r.Photos)
            .WithOne()
            .HasForeignKey(p => p.RequestId);
    }
}