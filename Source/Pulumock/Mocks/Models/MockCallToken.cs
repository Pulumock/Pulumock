using Pulumock.Extensions;

namespace Pulumock.Mocks.Models;

public record MockCallToken
{
    private Type? TypeToken { get; }
    private string? StringToken { get; }

    private MockCallToken(Type? typeToken, string? stringToken)
    {
        TypeToken = typeToken;
        StringToken = stringToken;
    }

    public static MockCallToken FromStringToken(string stringToken)
    {
        if (string.IsNullOrWhiteSpace(stringToken))
        {
            throw new ArgumentException("Token must be a non-empty string.", nameof(stringToken));
        }

        return new MockCallToken(null, stringToken);
    }

    public static MockCallToken FromTypeToken(Type typeToken)
    {
        ArgumentNullException.ThrowIfNull(typeToken);
        return new MockCallToken(typeToken, null);
    }

    public bool IsStringToken => StringToken is not null;
    public bool IsTypeToken => TypeToken is not null;
    
    public string StringTokenValue =>
        StringToken ?? throw new InvalidOperationException("This key does not represent a token.");

    public Type TypeTokenValue =>
        TypeToken ?? throw new InvalidOperationException("This key does not represent a type.");
    
    public bool ConflictsWith(MockCallToken other)
    {
        if (IsStringToken && other.IsStringToken)
        {
            return string.Equals(StringTokenValue, other.StringTokenValue, StringComparison.Ordinal);
        }

        if (IsTypeToken && other.IsTypeToken)
        {
            return TypeTokenValue == other.TypeTokenValue;
        }
        
        if (IsTypeToken && other.IsStringToken)
        {
            return TypeTokenValue.MatchesCallTypeToken(other.StringTokenValue);
        }

        if (IsStringToken && other.IsTypeToken)
        {
            return other.TypeTokenValue.MatchesCallTypeToken(StringTokenValue);
        }

        return false;
    }
}
