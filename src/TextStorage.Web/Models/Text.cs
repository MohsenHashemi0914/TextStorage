namespace TextStorage.Web.Models;

public sealed class Text
{
    public long Id { get; set; }

    public required string Content { get; set; }

    public required string ShortenCode { get; set; }

    public string? Password { get; set; }

    public DateTime? ExpiresOn { get; set; }
}