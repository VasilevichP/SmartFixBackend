using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFix.Domain.Aggregates;

namespace SmartFix.Infrastructure.Persistence.Configurations;

public class RequestPhotoConfiguration:IEntityTypeConfiguration<RequestPhoto>
{
    public void Configure(EntityTypeBuilder<RequestPhoto> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.FileName).IsRequired().HasMaxLength(255);
        builder.Property(p => p.FilePath).IsRequired().HasMaxLength(500);
    }
}