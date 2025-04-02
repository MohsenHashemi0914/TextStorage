using Microsoft.EntityFrameworkCore;
using TextStorage.Web.Persistence;

namespace TextStorage.Web.Workers;

public sealed class TextStorageCleanupBacgroundService(IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<TextStorageDbContext>();

            foreach (var prefix in context.GetAllPrefixes())
            {
                context.SetConnectionStringByPrefix(prefix);
                var countOfDeletedRows = await context.Texts.Where(x => x.ExpiresOn < DateTime.UtcNow).ExecuteDeleteAsync();
                await Task.Delay(3000);
            }
        }
    }
}