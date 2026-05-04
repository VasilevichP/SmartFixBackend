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
    public DbSet<Client> Clients { get; set; }
    public DbSet<Manager> Managers { get; set; }
    public DbSet<Master> Masters { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<ServiceCategory> ServiceCategories { get; set; }
    public DbSet<DeviceType> DeviceTypes { get; set; }
    public DbSet<Manufacturer> Manufacturers { get; set; }
    public DbSet<DeviceModel> DeviceModels { get; set; }
    public DbSet<PromoCode> PromoCodes { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<DayOfWeekDiscount> DayOfWeekDiscounts { get; set; }
    public DbSet<RequestsCountDiscount> RequestsCountDiscounts { get; set; }
    public DbSet<RequestSumDiscount> RequestSumDiscounts { get; set; }
    public DbSet<Request> Requests { get; set; }
    public DbSet<RequestService> RequestServices { get; set; }
    public DbSet<StatusHistory> StatusHistories { get; set; }
    public DbSet<RequestPhoto> RequestPhotos { get; set; }
    public DbSet<RequestDiscount> RequestDiscounts { get; set; }
    
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}