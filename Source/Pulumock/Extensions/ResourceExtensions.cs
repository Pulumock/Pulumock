using System.Collections.Immutable;
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
    public static T GetResourceByLogicalName<T>(this ImmutableArray<Resource> resources, string logicalName) where T : Resource =>
        resources
            .OfType<T>()
            .Single(x => x.GetResourceName().Equals(logicalName, StringComparison.Ordinal));
}
