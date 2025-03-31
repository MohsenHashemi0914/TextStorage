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

    public DbSet<Text> Texts { get; set; }

    public string ConnectionPrefix => _tenant.GetConnectionPrefix();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Text>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.ShortenCode).IsUnique();
            entity.Property(x => x.Password).HasMaxLength(20);
            entity.Property(x => x.ShortenCode).HasMaxLength(1000);
        });

        base.OnModelCreating(modelBuilder);
    }
}