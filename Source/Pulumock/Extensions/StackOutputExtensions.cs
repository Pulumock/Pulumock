using Pulumi;

namespace Pulumock.Extensions;

public static class StackOutputExtensions
{
    public static async Task<T> RequireValueAsync<T>(this IDictionary<string, object?> stackOutputs, string key)
    {
        if (!stackOutputs.TryGetValue(key, out object? output) || output is null)
        {
            throw new InvalidOperationException($"Output '{key}' was not found.");
        }

        return output switch
        {
            T directValue => directValue,
            Output<T> typedOutput => await typedOutput.GetValueAsync(),
            _ => throw new InvalidOperationException(
                $"Output '{key}' was not of expected type '{typeof(T).Name}' or 'Output<{typeof(T).Name}>'")
        };
    }
    
    public static async Task<T?> GetValueAsync<T>(this IDictionary<string, object?> stackOutputs, string key)
    {
        if (!stackOutputs.TryGetValue(key, out object? output) || output is null)
        {
            throw new InvalidOperationException($"Output '{key}' was not found.");
        }

        return output switch
        {
            T directValue => directValue,
            Output<T> typedOutput => await typedOutput.GetValueAsync(),
            _ => default
        };
    }
}
