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
    public DbSet<DeviceType> DeviceTypes { get; set; }
    public DbSet<Manufacturer> Manufacturers { get; set; }
    public DbSet<DeviceModel> DeviceModels { get; set; }
    public DbSet<Request> Requests { get; set; }
    public DbSet<StatusHistory> StatusHistories { get; set; }
    public DbSet<RequestPhoto> RequestPhotos { get; set; }
    public DbSet<Specialist> Specialists { get; set; }
    public DbSet<Review> Reviews { get; set; }

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

            builder.Property(u => u.Name).HasMaxLength(150);
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
            
            builder.HasCheckConstraint("CK_Services_WarrantyPeriod",
                "WarrantyPeriod > 0");
        });
        
        modelBuilder.Entity<Specialist>(builder =>
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.FullName).IsRequired().HasMaxLength(255);
        });
        
        modelBuilder.Entity<DeviceType>(builder =>
        {
            builder.HasKey(dt => dt.Id);
            builder.Property(dt => dt.Name).IsRequired().HasMaxLength(100);
        });
        modelBuilder.Entity<Manufacturer>(b => {
            b.HasKey(m => m.Id);
            b.Property(m => m.Name).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<DeviceModel>(b => {
            b.HasKey(m => m.Id);
            b.Property(m => m.Name).IsRequired().HasMaxLength(100);
            
            b.HasOne(m => m.Manufacturer)
                .WithMany()
                .HasForeignKey(m => m.ManufacturerId);

            b.HasOne(m => m.DeviceType)
                .WithMany()
                .HasForeignKey(m => m.DeviceTypeId);
        });

        modelBuilder.Entity<Request>(builder =>
        {
            builder.Property(r => r.DeviceModelName).IsRequired().HasMaxLength(150);
            
            builder.HasOne(r => r.Service)
                .WithMany()
                .HasForeignKey(r => r.ServiceId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

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
        });
        
        modelBuilder.Entity<RequestPhoto>(builder =>
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.FileName).IsRequired().HasMaxLength(255);
            builder.Property(p => p.FilePath).IsRequired().HasMaxLength(500);
        });
        
        modelBuilder.Entity<StatusHistory>(builder =>
        {
            builder.HasKey(h => h.Id);
            builder.Property(h => h.Status)
                .HasConversion<string>()
                .HasMaxLength(50);
        });
        modelBuilder.Entity<Review>(builder =>
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Comment).HasMaxLength(1000);
            builder.Property(r => r.Rating).IsRequired();
            
            builder.HasOne(r => r.Client)
                .WithMany()
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.Restrict); 
            
            builder.HasOne(r => r.Service)
                .WithMany(s => s.Reviews)
                .HasForeignKey(r => r.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}