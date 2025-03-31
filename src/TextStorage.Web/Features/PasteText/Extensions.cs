using TextStorage.Web.Models;
using TextStorage.Web.Persistence;

namespace TextStorage.Web.Features.PasteText;

public static class Extensions
{
    public static void MapCreateTextEndpouint(this IEndpointRouteBuilder endpoint)
    {
        endpoint.MapPost("/texts/paste", async (CreateTextRequestModel request, TextStorageDbContext context) =>
        {
            Text text = new()
            {
                Content = request.Content,
                Password = request.Password,
                ExpiresOn = request.ExpiresOn,
                ShortenCode = $"{context.ConnectionPrefix}-{Random.Shared.Next(111111, int.MaxValue)}"
            };

            await context.AddAsync(text);
            await context.SaveChangesAsync();

            return Results.Ok(text);
        });
    }
}

public sealed record CreateTextRequestModel(string Content, string? Password, DateTime? ExpiresOn);