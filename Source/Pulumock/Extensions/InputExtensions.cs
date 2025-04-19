using System.Collections.Immutable;
using Pulumock.Mocks.Models;

namespace Pulumock.Extensions;

/// <summary>
/// Provides extension methods for working with <see cref="Input"/>.
/// </summary>
public static class InputExtensions
{
    /// <summary>
    /// Retrieves an <see cref="Input"/> from the list by its unique identifier.
    /// </summary>
    /// <param name="inputs">The collection of inputs.</param>
    /// <param name="id">The identifier of the input to retrieve (logical name for resources or the token for calls).</param>
    /// <returns>The <see cref="Input"/> with the matching ID.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if no matching input is found, or if multiple matches exist.
    /// </exception>
    public static Input Get(this ImmutableList<Input> inputs, string id) =>
        inputs.Single(x => x.Id.Equals(id, StringComparison.Ordinal));
    
    /// <summary>
    /// Retrieves a required typed value from an input's dictionary by key.
    /// Throws if the key is missing or cannot be cast to the specified type.
    /// </summary>
    /// <typeparam name="T">The expected value type.</typeparam>
    /// <param name="inputs">The list of inputs.</param>
    /// <param name="id">The ID of the input to search (logical name for resources or the token for calls).</param>
    /// <param name="inputsKey">The key within the input's value dictionary to retrieve.</param>
    /// <returns>The value of type <typeparamref name="T"/> associated with the specified key.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if the key does not exist or is not of the expected type.
    /// </exception>
    public static T RequireValue<T>(this ImmutableList<Input> inputs, string id, string inputsKey)
    {
        Input input = inputs.Single(x => x.Id.Equals(id, StringComparison.Ordinal));
        if (!input.Inputs.TryGetValue(inputsKey, out object? value) || value is not T typedValue)
        {
            throw new KeyNotFoundException($"Input with key '{inputsKey}' was not found or is not of type {typeof(T).Name}.");
        }

        return typedValue;
    }
    
    /// <summary>
    /// Safely retrieves a typed value from an input's dictionary by key.
    /// Returns the default value if the key is missing or cannot be cast.
    /// </summary>
    /// <typeparam name="T">The expected value type.</typeparam>
    /// <param name="inputs">The list of inputs.</param>
    /// <param name="id">The ID of the input to search (logical name for resources or the token for calls).</param>
    /// <param name="inputsKey">The key within the input's value dictionary to retrieve.</param>
    /// <returns>
    /// The value of type <typeparamref name="T"/> if present and castable; otherwise, the default value for the type.
    /// </returns>
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
