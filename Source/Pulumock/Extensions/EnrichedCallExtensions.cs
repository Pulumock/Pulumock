using System.Collections.Immutable;
using System.Linq.Expressions;
using Pulumock.Mocks.Models;

namespace Pulumock.Extensions;

public static class EnrichedCallExtensions
{
    public static ImmutableList<EnrichedCall> GetMany(this ImmutableList<EnrichedCall> enrichedCalls, Type type) =>
        enrichedCalls
            .Where(x => type.MatchesCallTypeToken(x.Token))
            .ToImmutableList();
    
    public static ImmutableList<EnrichedCall> GetManyByValue<TProperty, TValue>(this ImmutableList<EnrichedCall> enrichedCalls, 
        Type type, Expression<Func<TProperty, object?>> propertySelector, TValue expectedValue)
        where TValue : notnull
    {
        string inputName = propertySelector.GetInputName();

        return enrichedCalls
            .Where(x => type.MatchesCallTypeToken(x.Token))
            .Where(x =>
                x.Inputs.TryGetValue(inputName, out object? value) &&
                value is TValue typedValue &&
                (typedValue is string s
                    ? s.Equals(expectedValue.ToString(), StringComparison.Ordinal)
                    : EqualityComparer<TValue>.Default.Equals(typedValue, expectedValue)))
            .ToImmutableList();
    }
    
    public static TValue RequireInputValue<TProperty, TValue>(this EnrichedCall enrichedCall,
        Expression<Func<TProperty, object?>> propertySelector)
    {
        string inputName = propertySelector.GetInputName();
        
        if (!enrichedCall.Inputs.TryGetValue(inputName, out object? value))
        {
            throw new KeyNotFoundException($"Input '{inputName}' not found in EnrichedCall '{enrichedCall.Token}'.");
        }

        if (value is not TValue typedValue)
        {
            throw new InvalidCastException($"Input '{inputName}' is not of type '{typeof(TValue).Name}'.");
        }
        
        return typedValue;
    }
    
    public static ImmutableList<TValue> RequireManyInputValues<TProperty, TValue>(this ImmutableList<EnrichedCall> enrichedCalls,
        Expression<Func<TProperty, object?>> propertySelector)
    {
        string inputName = propertySelector.GetInputName();

        var values = new List<TValue>();
        foreach (EnrichedCall enrichedCall in enrichedCalls)
        {
            if (!enrichedCall.Inputs.TryGetValue(inputName, out object? value))
            {
                throw new KeyNotFoundException($"Input '{inputName}' not found in EnrichedCall '{enrichedCall.Token}'.");
            }

            if (value is not TValue typedValue)
            {
                throw new InvalidCastException($"Input '{inputName}' is not of type '{typeof(TValue).Name}'.");
            }
            
            values.Add(typedValue);
        }
        
        
        return values.ToImmutableList();
    }
    
    public static ImmutableList<TValue> RequireManyOutputValues<TProperty, TValue>(this ImmutableList<EnrichedCall> enrichedCalls,
        Expression<Func<TProperty, object?>> propertySelector)
    {
        string outputName = propertySelector.GetOutputName();

        var values = new List<TValue>();
        foreach (EnrichedCall enrichedCall in enrichedCalls)
        {
            if (!enrichedCall.Outputs.TryGetValue(outputName, out object? value))
            {
                throw new KeyNotFoundException($"Output '{outputName}' not found in EnrichedCall '{enrichedCall.Token}'.");
            }

            if (value is not TValue typedValue)
            {
                throw new InvalidCastException($"Output '{outputName}' is not of type '{typeof(TValue).Name}'.");
            }
            
            values.Add(typedValue);
        }
        
        
        return values.ToImmutableList();
    }
}
