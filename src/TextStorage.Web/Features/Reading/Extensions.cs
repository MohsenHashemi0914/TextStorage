using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TextStorage.Web.Persistence;

namespace TextStorage.Web.Features.Reading;

public static class Extensions
{
    public static void MapReadingEndpouint(this IEndpointRouteBuilder endpoint)
    {
        endpoint.MapGet("/texts/{code}", async (
            [FromRoute(Name = nameof(code))] string code,
            ReadOnlyTextStorageDbContext context) =>
        {
            context.SetConnectionStringByPrefix(code[0]);
            var text = await context.Texts.AsNoTracking().FirstOrDefaultAsync(x => x.ShortenCode == code);
            return Results.Ok(text?.Content);
        });
    }
}