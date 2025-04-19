using System.Collections.Immutable;
using Pulumock.Mocks.Models;

namespace Pulumock.Extensions;

public static class InputExtensions
{
    public static Input Get(this ImmutableList<Input> inputs, string id) =>
        inputs.Single(x => x.Id.Equals(id, StringComparison.Ordinal));
    
    public static T RequireValue<T>(this ImmutableList<Input> inputs, string id, string inputsKey)
    {
        Input input = inputs.Single(x => x.Id.Equals(id, StringComparison.Ordinal));
        if (!input.Inputs.TryGetValue(inputsKey, out object? value) || value is not T typedValue)
        {
            throw new KeyNotFoundException($"Input with key '{inputsKey}' was not found or is not of type {typeof(T).Name}.");
        }

        return typedValue;
    }
    
    public static T? GetValue<T>(this ImmutableList<Input> inputs, string id, string inputsKey)
    {
        Input input = inputs.Single(x => x.Id.Equals(id, StringComparison.Ordinal));
        if (!input.Inputs.TryGetValue(inputsKey, out object? value) || value is not T typedValue)
        {
            return default;
        }

        return typedValue;
    }
}
