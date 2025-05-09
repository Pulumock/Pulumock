using Pulumi;

namespace Pulumock.Extensions;

/// <summary>
/// Provides extension methods for retrieving strongly-typed values from Pulumi stack outputs.
/// </summary>
public static class RegisteredOutputsExtensions
{
    /// <summary>
    /// Retrieves a required output value from the registered outputs by key.
    /// Supports both direct values and <see cref="Output{T}"/>-wrapped values.
    /// Throws if the key is missing or the value type is invalid.
    /// </summary>
    /// <typeparam name="T">The expected output value type.</typeparam>
    /// <param name="registeredOutputs">The registered outputs dictionary.</param>
    /// <param name="key">The key to retrieve from the outputs.</param>
    /// <returns>The resolved output value.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the key is missing or the value cannot be cast to <typeparamref name="T"/> or <see cref="Output{T}"/>.
    /// </exception>
    public static async Task<T> RequireValueAsync<T>(this IDictionary<string, object?> registeredOutputs, string key)
    {
        if (!registeredOutputs.TryGetValue(key, out object? output) || output is null)
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
    
    /// <summary>
    /// Attempts to retrieve an output value from the registered outputs by key.
    /// Supports both direct values and <see cref="Output{T}"/>-wrapped values.
    /// </summary>
    /// <typeparam name="T">The expected output value type.</typeparam>
    /// <param name="registeredOutputs">The registered outputs dictionary.</param>
    /// <param name="key">The key to retrieve from the outputs.</param>
    /// <returns>The resolved output value, or <c>default</c> if not found or mismatched type.</returns>
    public static async Task<T?> GetValueAsync<T>(this IDictionary<string, object?> registeredOutputs, string key)
    {
        if (!registeredOutputs.TryGetValue(key, out object? output) || output is null)
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
