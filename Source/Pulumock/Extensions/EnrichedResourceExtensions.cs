using System.Collections.Immutable;
using System.Linq.Expressions;
using Pulumock.Mocks.Models;

namespace Pulumock.Extensions;

/// <summary>
/// Provides extension methods for working with <see cref="EnrichedResource"/>.
/// </summary>
public static class EnrichedResourceExtensions
{
    /// <summary>
    /// Returns a single <see cref="EnrichedResource"/> of type <typeparamref name="TResourceType"/> by its logical name.
    /// Throws if not found or if multiple matches exist.
    /// </summary>
    public static EnrichedResource Require<TResourceType>(this ImmutableList<EnrichedResource> enrichedResources, string logicalName) =>
        enrichedResources.Single(x => typeof(TResourceType).MatchesResourceTypeToken(x.TypeToken) && 
                                      x.LogicalName.Equals(logicalName, StringComparison.Ordinal));
    
    /// <summary>
    /// Returns a single <see cref="EnrichedResource"/> of type <typeparamref name="TResourceType"/>
    /// Throws if not found or if multiple matches exist.
    /// </summary>
    public static EnrichedResource Require<TResourceType>(this ImmutableList<EnrichedResource> enrichedResources) =>
        enrichedResources.Single(x => typeof(TResourceType).MatchesResourceTypeToken(x.TypeToken));
    
    /// <summary>
    /// Returns an <see cref="EnrichedResource"/> of type <typeparamref name="TResourceType"/>, or <c>null</c> if not found.
    /// Throws if multiple matches exist.
    /// </summary>
    public static EnrichedResource? Get<TResourceType>(this ImmutableList<EnrichedResource> enrichedResources) =>
        enrichedResources.SingleOrDefault(x => typeof(TResourceType).MatchesResourceTypeToken(x.TypeToken));
    
    /// <summary>
    /// Returns an <see cref="EnrichedResource"/> of type <typeparamref name="TResourceType"/> by its logical name, or <c>null</c> if not found.
    /// Throws if multiple matches exist.
    /// </summary>
    public static EnrichedResource? Get<TResourceType>(this ImmutableList<EnrichedResource> enrichedResources, string logicalName) =>
        enrichedResources.SingleOrDefault(x => typeof(TResourceType).MatchesResourceTypeToken(x.TypeToken) && 
                                               x.LogicalName.Equals(logicalName, StringComparison.Ordinal));
    
    /// <summary>
    /// Returns all resources matching the specified resource type.
    /// </summary>
    public static ImmutableList<EnrichedResource> GetMany<TResourceType>(this ImmutableList<EnrichedResource> enrichedResources) =>
        enrichedResources
            .Where(x => typeof(TResourceType).MatchesResourceTypeToken(x.TypeToken))
            .ToImmutableList();
    
    /// <summary>
    /// Retrieves a strongly-typed input value from a resource using a property expression.
    /// Throws if the input is missing or of the wrong type.
    /// </summary>
    public static TValue RequireInputValue<TProperty, TValue>(this EnrichedResource enrichedResource,
        Expression<Func<TProperty, object?>> propertySelector)
    {
        string inputName = propertySelector.GetInputName();
        
        if (!enrichedResource.Inputs.TryGetValue(inputName, out object? value))
        {
            throw new KeyNotFoundException($"Input '{inputName}' not found in EnrichedResource '{enrichedResource.LogicalName}'.");
        }

        if (value is not TValue typedValue)
        {
            throw new InvalidCastException($"Input '{inputName}' is not of type '{typeof(TValue).Name}'.");
        }
        
        return typedValue;
    }
    
    /// <summary>
    /// Retrieves a nested input value from a resource using parent and child property expressions.
    /// Throws if the input is missing or of the wrong type.
    /// </summary>
    public static TValue RequireInputValue<TParent, TChild, TValue>(this EnrichedResource enrichedResource,
        Expression<Func<TParent, object?>> parentPropertySelector,
        Expression<Func<TChild, object?>> nestedPropertySelector)
    {
        string parentInputName = parentPropertySelector.GetInputName();
        if (!enrichedResource.Inputs.TryGetValue(parentInputName, out object? parentValue))
        {
            throw new KeyNotFoundException($"Input '{parentInputName}' not found in EnrichedResource '{enrichedResource.LogicalName}'.");
        }
        
        if (parentValue is not IDictionary<string, object> parentDictionary)
        {
            throw new InvalidCastException($"Parent input '{parentInputName}' is not a dictionary.");
        }

        string nestedInputName = nestedPropertySelector.GetInputName();
        if (!parentDictionary.TryGetValue(nestedInputName, out object? nestedValue))
        {
            throw new KeyNotFoundException($"Nested input '{nestedInputName}' not found inside '{parentInputName}'.");
        }

        if (nestedValue is not TValue typedValue)
        {
            throw new InvalidCastException($"Nested input '{nestedInputName}' is not of type '{typeof(TValue).Name}'.");
        }

        return typedValue;
    }
    
    /// <summary>
    /// Attempts to retrieve a strongly-typed input value from a resource using a property expression.
    /// Returns <c>default</c> if the value is missing or invalid.
    /// </summary>
    public static TValue? GetInputValue<TProperty, TValue>(this EnrichedResource enrichedResource,
        Expression<Func<TProperty, object?>> propertySelector)
    {
        string inputName = propertySelector.GetInputName();
        
        if (!enrichedResource.Inputs.TryGetValue(inputName, out object? value))
        {
            return default;
        }

        if (value is not TValue typedValue)
        {
            return default;
        }
        
        return typedValue;
    }
    
    /// <summary>
    /// Attempts to retrieve a nested input value from a resource using parent and child property expressions.
    /// Returns <c>default</c> if the value is missing or invalid.
    /// </summary>
    public static TValue? GetInputValue<TParent, TChild, TValue>(this EnrichedResource enrichedResource,
        Expression<Func<TParent, object?>> parentPropertySelector,
        Expression<Func<TChild, object?>> nestedPropertySelector)
    {
        string parentInputName = parentPropertySelector.GetInputName();
        if (!enrichedResource.Inputs.TryGetValue(parentInputName, out object? parentValue) 
            || parentValue is not IDictionary<string, object> parentDictionary)
        {
            return default;
        }
        
        string nestedInputName = nestedPropertySelector.GetInputName();
        if (!parentDictionary.TryGetValue(nestedInputName, out object? nestedValue)
            || nestedValue is not TValue typedValue)
        {
            return default;
        }

        return typedValue;
    }
}
