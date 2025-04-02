using Microsoft.EntityFrameworkCore;
using TextStorage.Web.Models;

namespace TextStorage.Web.Persistence;

public sealed class ReadOnlyTextStorageDbContext : DbContext
{
    private readonly LoadBalancer _loadBalancer;

    public ReadOnlyTextStorageDbContext(
        IServiceScopeFactory serviceScopeFactory,
        DbContextOptions<ReadOnlyTextStorageDbContext> options) : base(options)
    {
        using var scope = serviceScopeFactory.CreateScope();
        _loadBalancer = scope.ServiceProvider.GetRequiredService<LoadBalancer>();
        Database.SetConnectionString(_loadBalancer.GetTenant().ConnectionString);
    }

    public DbSet<Text> Texts { get; set; }

    public override int SaveChanges()
    {
        throw new InvalidOperationException();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        throw new InvalidOperationException();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException();
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException();
    }

    internal void SetConnectionStringByPrefix(char prefix)
    {
        var tenant = _loadBalancer.GetTenantByPrefix(prefix);
        Database.SetConnectionString(tenant.ConnectionString);
    }
}