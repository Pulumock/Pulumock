using System.Collections.Immutable;
using Pulumock.Extensions;

namespace Pulumock.Mocks.Models;

/// <summary>
/// Represents a mocked <see href="https://www.pulumi.com/docs/iac/concepts/resources/functions/">Pulumi provider function</see>
/// </summary>
/// <param name="Type">
/// The .NET <see cref="Type"/> that represents the Pulumi provider function being mocked.
/// </param>
/// <param name="MockOutputs">
/// A dictionary of mocked output values that the function is expected to return during testing.
/// These keys should match the output property names defined by the real Pulumi function.
/// </param>
public record MockCall(MockCallToken Token, ImmutableDictionary<string, object> MockOutputs);

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
