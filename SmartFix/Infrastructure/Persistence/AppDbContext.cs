using Microsoft.EntityFrameworkCore;
using SmartFix.Domain.Aggregates;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<ServiceCategory> ServiceCategories { get; set; }
    public DbSet<Request> Requests { get; set; }
    public DbSet<StatusHistory> StatusHistories { get; set; }
    public DbSet<Specialist> Specialists { get; set; }

    [Obsolete("Obsolete")]
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(builder =>
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Email).HasMaxLength(255).IsRequired();
            builder.HasIndex(u => u.Email).IsUnique();

            builder.Property(u => u.PasswordHash).IsRequired();

            builder.Property(u => u.Role)
                .HasConversion(
                    v => v.ToString(),
                    v => (Role)Enum.Parse(typeof(Role), v));

            builder.Property(u => u.FirstName).HasMaxLength(100);
            builder.Property(u => u.LastName).HasMaxLength(100);
            builder.Property(u => u.MiddleName).HasMaxLength(100);
            builder.Property(u => u.PhoneNumber).HasMaxLength(20);
        });

        modelBuilder.Entity<ServiceCategory>(builder =>
        {
            builder.HasKey(sc => sc.Id);
            builder.Property(sc => sc.Name).HasMaxLength(255).IsRequired();
        });

        modelBuilder.Entity<Service>(builder =>
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Name).HasMaxLength(255).IsRequired();
            builder.Property(s => s.Description).HasMaxLength(2000);
            builder.Property(s => s.Price).HasColumnType("decimal(18, 2)").IsRequired();
            builder.Property(s => s.IsAvailable).IsRequired();

            builder.HasOne(s => s.Category)
                .WithMany(sc => sc.Services)
                .HasForeignKey(s => s.CategoryId);
            builder.HasCheckConstraint("CK_Services_WarrantyPeriod",
                "WarrantyPeriod > 0");
        });
        
        modelBuilder.Entity<Specialist>(builder =>
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.FullName).IsRequired().HasMaxLength(255);
        });
    }
}