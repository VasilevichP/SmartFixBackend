using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFix.Domain.Aggregates;

namespace SmartFix.Infrastructure.Persistence.Configurations;

public class ServiceConfiguration:IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Name).HasMaxLength(255).IsRequired();
        builder.Property(s => s.Description).HasMaxLength(2000);
        builder.Property(s => s.Price).HasColumnType("decimal(18, 2)").IsRequired();
        builder.Property(s => s.IsAvailable).IsRequired();

        builder.HasOne(s => s.Category)
            .WithMany(sc => sc.Services)
            .HasForeignKey(s => s.CategoryId);
            
        builder.HasOne(s => s.DeviceType)
            .WithMany()
            .HasForeignKey(s => s.DeviceTypeId)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.HasOne(s => s.Manufacturer)
            .WithMany()
            .HasForeignKey(s => s.ManufacturerId)
            .IsRequired(false);
            
        builder.HasOne(s => s.DeviceModel)
            .WithMany()
            .HasForeignKey(s => s.DeviceModelId)
            .IsRequired(false);
    }
}