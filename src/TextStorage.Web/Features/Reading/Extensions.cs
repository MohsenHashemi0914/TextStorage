using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using TextStorage.Web.Persistence;

namespace TextStorage.Web.Features.Reading;

public static class Extensions
{
    public static void MapReadingEndpouint(this IEndpointRouteBuilder endpoint)
    {
        endpoint.MapGet("/texts/{code}", async (
            [FromRoute(Name = nameof(code))] string code,
            IDistributedCache distributedCache,
            IServiceScopeFactory scopeFactory) =>
        {
            // check code validation first!

            var content = await distributedCache.GetStringAsync(code);
            if (!string.IsNullOrWhiteSpace(content))
            {
                return Results.Ok(content);
            }
            
            using var scope = scopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<ReadOnlyTextStorageDbContext>();
            context.SetConnectionStringByPrefix(code[0]);

            var text = await context.Texts.FirstOrDefaultAsync(x => x.ShortenCode == code);
            if (text is null)
            {
                return Results.NotFound();
            }

            DistributedCacheEntryOptions cacheOptions = new()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };

            await distributedCache.SetStringAsync(code, text.Content, cacheOptions);
            return Results.Ok(text.Content);
        });
    }
}