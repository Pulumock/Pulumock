using System.Collections.Immutable;
using System.Linq.Expressions;
using Pulumi;

namespace Pulumock.Extensions;

/// <summary>
/// Provides extension methods for working with <see cref="Resource"/>.
/// </summary>
public static class ResourceExtensions
{
    /// <summary>
    /// Retrieves a resource of type <typeparamref name="T"/> from the collection by its logical name.
    /// </summary>
    /// <typeparam name="T">The expected resource type to return.</typeparam>
    /// <param name="resources">The collection of Pulumi resources.</param>
    /// <param name="logicalName">The logical name of the desired resource.</param>
    /// <returns>The matching resource of type <typeparamref name="T"/>.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if no matching resource is found, or if multiple matches exist.
    /// </exception>
    public static T Require<T>(this ImmutableArray<Resource> resources, string logicalName) where T : Resource =>
        resources
            .OfType<T>()
            .Single(x => x.GetResourceName().Equals(logicalName, StringComparison.Ordinal));
    
    public static T? Get<T>(this ImmutableArray<Resource> resources, string logicalName) where T : Resource =>
        resources
            .OfType<T>()
            .SingleOrDefault(x => x.GetResourceName().Equals(logicalName, StringComparison.Ordinal));
    
    public static ImmutableArray<T> GetMany<T>(this ImmutableArray<Resource> resources) where T : Resource =>
        resources
            .OfType<T>()
            .ToImmutableArray();
}
