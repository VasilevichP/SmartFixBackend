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
    public DbSet<RequestService> RequestServices { get; set; }
    public DbSet<StatusHistory> StatusHistories { get; set; }
    public DbSet<RequestPhoto> RequestPhotos { get; set; }
    public DbSet<Specialist> Specialists { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}