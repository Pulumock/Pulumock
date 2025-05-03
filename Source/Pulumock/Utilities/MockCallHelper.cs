using System.Collections.Immutable;
using Pulumock.Extensions;
using Pulumock.Mocks.Models;

namespace Pulumock.Utilities;

/// <summary>
/// Provides utility methods for <see cref="MockCall"/>.
/// </summary>
public static class MockCallHelper
{
    /// <summary>
    /// Ensures that a given token string is non-null and non-whitespace.
    /// </summary>
    /// <param name="token">The token string to validate.</param>
    /// <returns>The validated token string.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the token is null or whitespace.</exception>
    public static string GetToken(string? token) =>
        string.IsNullOrWhiteSpace(token) ? throw new ArgumentNullException(nameof(token)) : token;
    
    /// <summary>
    /// Attempts to retrieve a matching <see cref="MockCall"/> from a dictionary based on a string call token.
    /// Matches either a string-defined or type-defined <see cref="MockCallToken"/>.
    /// </summary>
    /// <param name="calls">The dictionary of registered mock calls.</param>
    /// <param name="callToken">The string token to match against.</param>
    /// <returns>The matching <see cref="MockCall"/>, or <c>null</c> if none is found.</returns>
    public static MockCall? GetOrDefault(ImmutableDictionary<MockCallToken, MockCall> calls, string callToken)
    {
        MockCall? match = calls
            .FirstOrDefault(kvp =>
                kvp.Key.IsStringToken &&
                string.Equals(kvp.Key.StringTokenValue, callToken, StringComparison.Ordinal))
            .Value;
        
        return match ?? calls
            .FirstOrDefault(kvp =>
                kvp.Key.IsTypeToken &&
                kvp.Key.TypeTokenValue.MatchesCallTypeToken(callToken))
            .Value;
    }
}
