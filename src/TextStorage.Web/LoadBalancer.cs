namespace TextStorage.Web;

public sealed class LoadBalancer(string[] connectionStrings)
{
    private const int FIRST_CHAR_NUMBER = 97; // equals a in ASCII
    private const int LAST_CHAR_NUMBER = 122; // equals z in ASCII

    private static readonly object _locker = new();
    private readonly int _length = connectionStrings.Length;
    private int _currentPosition = 1;

    public IReadOnlyList<char> GetAllPrefixes()
    {
        return [.. connectionStrings.Select((x, index) => (char)(index + FIRST_CHAR_NUMBER))];
    }

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

    public TenantPrincipal GetTenantByPrefix(char prefix)
    {
        var prefixCharacterNumber = (int)prefix;
        if(prefixCharacterNumber is < FIRST_CHAR_NUMBER or > LAST_CHAR_NUMBER) // a-z ASCII
        {
            throw new InvalidDataException();
        }

        var index = prefixCharacterNumber - FIRST_CHAR_NUMBER;
        return new()
        {
            Id = index + 1,
            ConnectionString = connectionStrings[index]
        };
    }

    public sealed record TenantPrincipal
    {
        public required int Id { get; init; }

        public required string ConnectionString { get; init; }

        public char ConnectionPrefix => (char)((Id - 1) + FIRST_CHAR_NUMBER);
    }
}