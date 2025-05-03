using System.Collections.Immutable;
using System.Linq.Expressions;
using Pulumock.Mocks.Models;

namespace Pulumock.Extensions;

/// <summary>
/// Provides extension methods for <see cref="EnrichedCall"/>.
/// </summary>
public static class EnrichedCallExtensions
{
    /// <summary>
    /// Filters the list of enriched calls to those matching a given function call type.
    /// </summary>
    /// <param name="enrichedCalls">The collection of calls to filter.</param>
    /// <param name="type">The provider function type to match.</param>
    /// <returns>A filtered list of matching calls.</returns>
    public static ImmutableList<EnrichedCall> GetMany(this ImmutableList<EnrichedCall> enrichedCalls, Type type) =>
        enrichedCalls
            .Where(x => type.MatchesCallTypeToken(x.Token))
            .ToImmutableList();
    
    /// <summary>
    /// Filters calls by function type and a specific input value using a strongly-typed property expression.
    /// </summary>
    /// <typeparam name="TProperty">The type containing the input property.</typeparam>
    /// <typeparam name="TValue">The expected value type.</typeparam>
    /// <param name="enrichedCalls">The collection of calls to filter.</param>
    /// <param name="type">The provider function type to match.</param>
    /// <param name="propertySelector">Expression selecting the input property to match.</param>
    /// <param name="expectedValue">The expected value of the input.</param>
    /// <returns>A filtered list of calls matching the type and input value.</returns>
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
    
    /// <summary>
    /// Retrieves a strongly-typed input value from an enriched call using an expression selector.
    /// </summary>
    /// <typeparam name="TProperty">The type containing the input property.</typeparam>
    /// <typeparam name="TValue">The expected value type.</typeparam>
    /// <param name="enrichedCall">The call to retrieve the input from.</param>
    /// <param name="propertySelector">Expression selecting the input property.</param>
    /// <returns>The input value.</returns>
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
    
    /// <summary>
    /// Retrieves multiple strongly-typed input values from a list of calls using an expression selector.
    /// </summary>
    /// <typeparam name="TProperty">The type containing the input property.</typeparam>
    /// <typeparam name="TValue">The expected value type.</typeparam>
    /// <param name="enrichedCalls">The calls to extract input values from.</param>
    /// <param name="propertySelector">Expression selecting the input property.</param>
    /// <returns>A list of extracted input values.</returns>
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
    
    /// <summary>
    /// Retrieves multiple strongly-typed output values from a list of calls using an expression selector.
    /// </summary>
    /// <typeparam name="TProperty">The type containing the output property.</typeparam>
    /// <typeparam name="TValue">The expected value type.</typeparam>
    /// <param name="enrichedCalls">The calls to extract output values from.</param>
    /// <param name="propertySelector">Expression selecting the output property.</param>
    /// <returns>A list of extracted output values.</returns>
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
