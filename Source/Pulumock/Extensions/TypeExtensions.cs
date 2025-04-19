using System.Linq.Expressions;
using System.Reflection;
using Pulumi;

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
    /// Retrieves the Pulumi output key name from a strongly typed expression pointing to a property or field.
    /// </summary>
    /// <typeparam name="T">The type containing the output member.</typeparam>
    /// <param name="propertySelector">
    /// An expression pointing to the output member (e.g., <c>x => x.SubscriptionId</c>).
    /// </param>
    /// <returns>
    /// The name of the output key to be used in Pulumi mocks. If the member has an <see cref="OutputAttribute"/>,
    /// its <c>Name</c> value is used; otherwise, the member name is converted to camelCase.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the expression is not a valid property or field selector.
    /// </exception>
    public static string GetOutputName<T>(this Expression<Func<T, object>> propertySelector)
    {
        MemberInfo member = propertySelector.Body switch
        {
            MemberExpression m => m.Member,
            UnaryExpression { Operand: MemberExpression m } => m.Member,
            _ => throw new ArgumentException("Invalid property selector")
        };

        return member.GetCustomAttribute<OutputAttribute>()?.Name ?? member.Name.ToCamelCase();
    }
    
    /// <summary>
    /// The name of the property on Pulumi resource attributes that contains the type token.
    /// </summary>
    private const string PulumiTypeTokenPropertyName = "Type";
}
