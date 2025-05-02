using Pulumi;

namespace Pulumock.Extensions;

public static class StackOutputExtensions
{
    public static async Task<T> RequireValueAsync<T>(this IDictionary<string, object?> stackOutputs, string key)
    {
        if (!stackOutputs.TryGetValue(key, out object? output) 
            || output is not Output<T> typedOutput)
        {
            throw new InvalidOperationException($"{key} was not found or was not of expected type Output<{typeof(T).Name}>");
        }

        return await typedOutput.GetValueAsync();
    }
    
    public static async Task<T?> GetValueAsync<T>(this IDictionary<string, object> stackOutputs, string key)
    {
        if (!stackOutputs.TryGetValue(key, out object? output) 
            || output is not Output<T> typedOutput)
        {
            return default;
        }

        return await typedOutput.GetValueAsync();
    }
}
