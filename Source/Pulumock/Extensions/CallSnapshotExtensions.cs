using System.Collections.Immutable;
using System.Linq.Expressions;
using Pulumock.Mocks.Models;

namespace Pulumock.Extensions;

public static class CallSnapshotExtensions
{
    public static ImmutableList<CallSnapshot> GetMany(this ImmutableList<CallSnapshot> callSnapshots, Type type) =>
        callSnapshots
            .Where(x => type.MatchesCallTypeToken(x.Token))
            .ToImmutableList();
    
    public static TValue RequireInputValue<TProperty, TValue>(this CallSnapshot callSnapshot,
        Expression<Func<TProperty, object?>> propertySelector)
    {
        string inputName = propertySelector.GetInputName();
        
        if (!callSnapshot.Inputs.TryGetValue(inputName, out object? value))
        {
            throw new KeyNotFoundException($"Input '{inputName}' not found in CallSnapshot '{callSnapshot.Token}'.");
        }

        if (value is not TValue typedValue)
        {
            throw new InvalidCastException($"Input '{inputName}' is not of type '{typeof(TValue).Name}'.");
        }
        
        return typedValue;
    }
    
    public static ImmutableList<TValue> RequireManyInputValues<TProperty, TValue>(this ImmutableList<CallSnapshot> callSnapshots,
        Expression<Func<TProperty, object?>> propertySelector)
    {
        string inputName = propertySelector.GetInputName();

        var values = new List<TValue>();
        foreach (CallSnapshot callSnapshot in callSnapshots)
        {
            if (!callSnapshot.Inputs.TryGetValue(inputName, out object? value))
            {
                throw new KeyNotFoundException($"Input '{inputName}' not found in CallSnapshot '{callSnapshot.Token}'.");
            }

            if (value is not TValue typedValue)
            {
                throw new InvalidCastException($"Input '{inputName}' is not of type '{typeof(TValue).Name}'.");
            }
            
            values.Add(typedValue);
        }
        
        
        return values.ToImmutableList();
    }
}
