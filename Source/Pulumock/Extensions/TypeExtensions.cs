namespace Pulumock.Extensions;

/// <summary>
/// Provides extension methods for working with <see cref="Type"/>.
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// Determines whether the given type has a Pulumi resource attribute with the specified type token.
    /// </summary>
    /// <param name="type">The type to inspect.</param>
    /// <param name="token">The Pulumi type token to match.</param>
    /// <returns><c>true</c> if the type has an attribute with a matching type token; otherwise, <c>false</c>.</returns>
    public static bool MatchesResourceTypeToken(this Type type, string? token) => 
        !string.IsNullOrWhiteSpace(token) && 
        type.GetResourceTypeTokens().Contains(token);
    
    /// <summary>
    /// Determines whether the given provider function type is referenced by the specified call type token,
    /// based on a case-insensitive match of the type's name within the token.
    /// </summary>
    /// <param name="type">The type to inspect.</param>
    /// <param name="token">The Pulumi provider function type token to match.</param>
    /// <returns><c>true</c> if the token contains the type name; otherwise, <c>false</c>.</returns>
    public static bool MatchesCallTypeToken(this Type type, string? token) => 
        !string.IsNullOrWhiteSpace(token) && 
        token.Contains(type.Name, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Retrieves all Pulumi type tokens declared via resource type attributes on the given type (primary token or any alias).
    /// </summary>
    /// <param name="type">The type to inspect.</param>
    /// <returns>A sequence of Pulumi type tokens defined on the type via resource attributes.</returns>
    private static IEnumerable<string> GetResourceTypeTokens(this Type type) =>
        type
            .GetCustomAttributes(inherit: false)
            .Select(attr => attr
                .GetType()
                .GetProperty(PulumiTypeTokenPropertyName)?
                .GetValue(attr))
            .OfType<string>()
            .Where(token => !string.IsNullOrWhiteSpace(token));
    
    /// <summary>
    /// The name of the property on Pulumi resource attributes that contains the type token.
    /// </summary>
    private const string PulumiTypeTokenPropertyName = "Type";
}
