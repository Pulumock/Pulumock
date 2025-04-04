using System.Reflection;

namespace Pulumock.Extensions;

public static class MemberInfoExtensions
{
        
    /// <summary>
    /// Determines whether the given type has a Pulumi resource attribute with the specified type token.
    /// </summary>
    /// <param name="type">The type to inspect.</param>
    /// <param name="token">The Pulumi type token to match.</param>
    /// <returns><c>true</c> if the type has an attribute with a matching type token; otherwise, <c>false</c>.</returns>
    public static bool MatchesPulumiTypeToken(this MemberInfo type, string? token) => 
        !string.IsNullOrWhiteSpace(token) && 
        type.GetPulumiTypeTokens().Contains(token);

    /// <summary>
    /// Retrieves all Pulumi type tokens declared via resource type attributes on the given type (primary token or any alias).
    /// </summary>
    /// <param name="type">The type to inspect.</param>
    /// <returns>A sequence of Pulumi type tokens defined on the type via resource attributes.</returns>
    private static IEnumerable<string> GetPulumiTypeTokens(this MemberInfo type) =>
        type
            .GetCustomAttributes(inherit: false)
            .Select(attr => attr
                .GetType()
                .GetProperty(PulumiTypeTokenPropertyName)?
                .GetValue(attr) as string)
            .OfType<string>()
            .Where(token => !string.IsNullOrWhiteSpace(token));
    
    /// <summary>
    /// The name of the property on Pulumi resource attributes that contains the type token.
    /// </summary>
    private const string PulumiTypeTokenPropertyName = "Type";
}
