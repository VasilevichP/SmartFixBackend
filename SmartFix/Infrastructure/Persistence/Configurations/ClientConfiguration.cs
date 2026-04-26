using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartFix.Domain.Aggregates;

namespace SmartFix.Infrastructure.Persistence.Configurations;

public class ClientConfiguration: IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.Property(u => u.Name).HasMaxLength(150);
        builder.Property(u => u.PhoneNumber).HasMaxLength(20);
    }
}