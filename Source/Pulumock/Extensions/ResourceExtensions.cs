using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection;
using Pulumi;

namespace Pulumock.Extensions;

/// <summary>
/// Provides extension methods for working with <see cref="Resource"/>.
/// </summary>
public static class ResourceExtensions
{
    /// <summary>
    /// Retrieves a resource of type <typeparamref name="T"/> by its logical name.
    /// Throws if not found or if multiple matches exist.
    /// </summary>
    public static T Require<T>(this ImmutableArray<Resource> resources, string logicalName) where T : Resource =>
        resources
            .OfType<T>()
            .Single(x => x.GetResourceName().Equals(logicalName, StringComparison.Ordinal));
    
    /// <summary>
    /// Retrieves a single resource of type <typeparamref name="T"/>. 
    /// Throws if not found or if multiple matches exist.
    /// </summary>
    public static T Require<T>(this ImmutableArray<Resource> resources) where T : Resource =>
        resources
            .OfType<T>()
            .Single();
    
    /// <summary>
    /// Returns a resource of type <typeparamref name="T"/> by its logical name, or <c>null</c> if not found.
    /// Throws if multiple matches exist.
    /// </summary>
    public static T? Get<T>(this ImmutableArray<Resource> resources, string logicalName) where T : Resource =>
        resources
            .OfType<T>()
            .SingleOrDefault(x => x.GetResourceName().Equals(logicalName, StringComparison.Ordinal));
    
    /// <summary>
    /// Returns a single resource of type <typeparamref name="T"/>, or <c>null</c> if not found.
    /// Throws if multiple matches exist.
    /// </summary>
    public static T? Get<T>(this ImmutableArray<Resource> resources) where T : Resource =>
        resources
            .OfType<T>()
            .SingleOrDefault();
    
    /// <summary>
    /// Returns all resources of type <typeparamref name="T"/> from the collection.
    /// </summary>
    public static ImmutableArray<T> GetMany<T>(this ImmutableArray<Resource> resources) where T : Resource =>
        resources
            .OfType<T>()
            .ToImmutableArray();
    
    /// <summary>
    /// Retrieves and resolves an output value from each resource using the provided output selector.
    /// </summary>
    public static async Task<T[]> GetManyValuesAsync<TResource, T>(this ImmutableArray<TResource> resources,
        Expression<Func<TResource, Output<T>>> outputSelector)
        where TResource : Resource
    {
        Func<TResource, Output<T>> compiledSelector = outputSelector.Compile();

        IEnumerable<Task<T>> tasks = resources.Select(resource =>
        {
            Output<T> output = compiledSelector(resource);
            return output.GetValueAsync();
        });

        return await Task.WhenAll(tasks);
    }
    
    /// <summary>
    /// Determines whether a resource is a direct child of the specified parent.
    /// </summary>
    public static bool IsChildOf(this Resource child, Resource potentialParent)
    {
        PropertyInfo? childResourcesField = typeof(Resource)
            .GetProperty("ChildResources", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        if (childResourcesField is null)
        {
            throw new InvalidOperationException("Could not reflect ChildResources field.");
        }

        var children = childResourcesField.GetValue(potentialParent) as IEnumerable<Resource>;

        return children?.Contains(child) ?? false;
    }
    
    /// <summary>
    /// Determines whether all specified resources are direct children of the given parent resource.
    /// </summary>
    public static bool HasChildren(this Resource parent, IEnumerable<Resource> potentialChildren)
    {
        PropertyInfo? childResourcesProperty = typeof(Resource)
            .GetProperty("ChildResources", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        if (childResourcesProperty is null)
        {
            throw new InvalidOperationException("Could not reflect 'ChildResources' property.");
        }

        if (childResourcesProperty.GetValue(parent) is not IEnumerable<Resource> children)
        {
            return false;
        }

        return potentialChildren.All(child => children.Contains(child));
    }
}
