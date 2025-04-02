namespace TextStorage.Web;

public sealed class AppSettings
{
    public required string RedisConnection { get; set; }

    public required Connections ConnectionStrings { get; set; }

    public sealed class Connections
    {
        public required string Master1 { get; set; }
        public required string Master2 { get; set; }
        public required string Master3 { get; set; }
    }
}