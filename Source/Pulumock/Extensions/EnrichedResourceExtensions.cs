using System.Collections.Immutable;
using System.Linq.Expressions;
using Pulumock.Mocks.Models;

namespace Pulumock.Extensions;

/// <summary>
/// Provides extension methods for working with <see cref="EnrichedResource"/>.
/// </summary>
public static class EnrichedResourceExtensions
{
    public static EnrichedResource Require(this ImmutableList<EnrichedResource> enrichedResources, string logicalName) =>
        enrichedResources.Single(x => x.LogicalName.Equals(logicalName, StringComparison.Ordinal));
    
    public static EnrichedResource? Get(this ImmutableList<EnrichedResource> enrichedResources, string logicalName) =>
        enrichedResources.SingleOrDefault(x => x.LogicalName.Equals(logicalName, StringComparison.Ordinal));
    
    public static ImmutableList<EnrichedResource> GetMany<TResourceType>(this ImmutableList<EnrichedResource> enrichedResources) =>
        enrichedResources
            .Where(x => typeof(TResourceType).MatchesResourceTypeToken(x.TypeToken))
            .ToImmutableList();
    
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
