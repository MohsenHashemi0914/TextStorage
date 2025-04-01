namespace TextStorage.Web;

public sealed class LoadBalancer(string[] connectionStrings)
{
    private static readonly object _locker = new();
    private readonly int _length = connectionStrings.Length;
    private int _currentPosition = 1;

    public TenantPrincipal GetTenant()
    {
        lock (_locker)
        {
            var tenantId = _currentPosition;
            var connectionString = connectionStrings[(_currentPosition - 1)];

            _currentPosition++;
            if (_currentPosition > _length)
            {
                _currentPosition = 1;
            }

            return new()
            {
                Id = tenantId,
                ConnectionString = connectionString
            };
        }
    }

    public sealed record TenantPrincipal
    {
        public required int Id { get; init; }

        public required string ConnectionString { get; init; }

        public string GetConnectionPrefix()
        {
            return Id switch
            {
                1 => "1M",
                2 => "2M",
                3 => "3M",
                _ => throw new InvalidDataException()
            };
        }
    }
}
