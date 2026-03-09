using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFix.Domain.Aggregates;

namespace SmartFix.Infrastructure.Persistence.Configurations;

public class DeviceModelConfiguration:IEntityTypeConfiguration<DeviceModel>
{
    public void Configure(EntityTypeBuilder<DeviceModel> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Name).IsRequired().HasMaxLength(100);
            
        builder.HasOne(m => m.Manufacturer)
            .WithMany()
            .HasForeignKey(m => m.ManufacturerId);

        builder.HasOne(m => m.DeviceType)
            .WithMany()
            .HasForeignKey(m => m.DeviceTypeId);
    }
}