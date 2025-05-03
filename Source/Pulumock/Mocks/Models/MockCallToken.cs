using Pulumock.Extensions;

namespace Pulumock.Mocks.Models;

/// <summary>
/// Represents a flexible token identifier for a mocked Pulumi function call.
/// Can be defined using either a <see cref="Type"/> reference or a string token.
/// </summary>
public record MockCallToken
{
    private Type? TypeToken { get; }
    private string? StringToken { get; }

    private MockCallToken(Type? typeToken, string? stringToken)
    {
        TypeToken = typeToken;
        StringToken = stringToken;
    }

    /// <summary>
    /// Creates a token identifier from a string-based token.
    /// </summary>
    /// <param name="stringToken">The fully qualified function token (e.g., <c>"azure-native:authorization:getRoleDefinition"</c>).</param>
    /// <returns>A <see cref="MockCallToken"/> representing the string token.</returns>
    /// <exception cref="ArgumentException">Thrown if the token is null or empty.</exception>
    public static MockCallToken FromStringToken(string stringToken)
    {
        if (string.IsNullOrWhiteSpace(stringToken))
        {
            throw new ArgumentException("Token must be a non-empty string.", nameof(stringToken));
        }

        return new MockCallToken(null, stringToken);
    }

    /// <summary>
    /// Creates a token identifier from a function call <see cref="Type"/>.
    /// </summary>
    /// <param name="typeToken">The .NET type representing the function call.</param>
    /// <returns>A <see cref="MockCallToken"/> representing the type token.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the type is null.</exception>
    public static MockCallToken FromTypeToken(Type typeToken)
    {
        ArgumentNullException.ThrowIfNull(typeToken);
        return new MockCallToken(typeToken, null);
    }

    /// <summary>
    /// Indicates whether the token is based on a string value.
    /// </summary>
    public bool IsStringToken => StringToken is not null;
    
    /// <summary>
    /// Indicates whether the token is based on a <see cref="Type"/>.
    /// </summary>
    public bool IsTypeToken => TypeToken is not null;
    
    /// <summary>
    /// Gets the string token value, or throws if the token is not string-based.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the token is not a string token.</exception>
    public string StringTokenValue =>
        StringToken ?? throw new InvalidOperationException("This key does not represent a token.");

    /// <summary>
    /// Gets the <see cref="Type"/> token value, or throws if the token is not type-based.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the token is not a type token.</exception>
    public Type TypeTokenValue =>
        TypeToken ?? throw new InvalidOperationException("This key does not represent a type.");
    
    /// <summary>
    /// Determines whether this token conflicts with another token by matching
    /// either their string representations or their resolved types.
    /// </summary>
    /// <param name="other">The other <see cref="MockCallToken"/> to compare.</param>
    /// <returns><c>true</c> if the tokens represent the same function call; otherwise, <c>false</c>.</returns>
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
