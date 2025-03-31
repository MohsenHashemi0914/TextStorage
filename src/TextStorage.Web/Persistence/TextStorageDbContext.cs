using Microsoft.EntityFrameworkCore;
using TextStorage.Web.Models;
using TenantPrincipal = TextStorage.Web.LoadBalancer.TenantPrincipal;

namespace TextStorage.Web.Persistence;

public sealed class TextStorageDbContext : DbContext
{
    private readonly TenantPrincipal _tenant;

    public TextStorageDbContext(
        IServiceScopeFactory serviceScopeFactory,
        DbContextOptions<TextStorageDbContext> options) : base(options)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var loadBalancer = scope.ServiceProvider.GetRequiredService<LoadBalancer>();
        _tenant = loadBalancer.GetTenant();
        Database.SetConnectionString(_tenant.ConnectionString);
    }

    public string ConnectionPrefix => _tenant.GetConnectionPrefix();

    public DbSet<Text> Texts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Text>().HasKey(x => x.Id);
        modelBuilder.Entity<Text>().Property(x => x.ShortenCode)
                                   .HasMaxLength(1000);
        modelBuilder.Entity<Text>().HasIndex(x => x.ShortenCode)
                                   .IsUnique();
        modelBuilder.Entity<Text>().Property(x => x.Password)
                                   .HasMaxLength(20);

        base.OnModelCreating(modelBuilder);
    }
}