using System.Collections.Immutable;
using System.Linq.Expressions;
using Pulumi;
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
        Input? input = inputs.SingleOrDefault(x => x.Id.Equals(inputsKey, StringComparison.Ordinal));
        if (input is null 
            || !input.Inputs.TryGetValue(inputsKey, out object? value) 
            || value is not T typedValue)
        {
            return default;
        }

        return typedValue;
    }
    
    /// <summary>
    /// Retrieves a required input value from a Pulumi resource mock based on a strongly typed input property selector.
    /// </summary>
    /// <typeparam name="TResource">
    /// The Pulumi <see cref="ResourceArgs"/> type that defines the resource inputs (e.g., <c>ResourceGroupArgs</c>).
    /// </typeparam>
    /// <typeparam name="TValue">The expected type of the value to retrieve.</typeparam>
    /// <param name="inputs">The list of mocked inputs for the Pulumi test environment.</param>
    /// <param name="logicalName">The logical name (ID) of the resource to match in the input list.</param>
    /// <param name="propertySelector">
    /// An expression selecting the desired input property (e.g., <c>x => x.Location</c>).
    /// </param>
    /// <returns>The value of type <typeparamref name="TValue"/> from the matching resource input.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if the resource with the specified <paramref name="logicalName"/> is not found,
    /// or if the value for the selected input property is missing or not of the expected type.
    /// </exception>
    public static TValue RequireValue<TResource, TValue>(this ImmutableList<Input> inputs,
        string logicalName, Expression<Func<TResource, object?>> propertySelector)
        where TResource : ResourceArgs
    {
        string outputKey = propertySelector.GetInputName();
        
        Input input = inputs.Single(x => x.Id.Equals(logicalName, StringComparison.Ordinal));
        if (!input.Inputs.TryGetValue(outputKey, out object? value) || value is not TValue typedValue)
        {
            throw new KeyNotFoundException($"Value for property '{outputKey}' on resource '{logicalName}' was not found or is not of type {typeof(TValue).Name}.");
        }

        return typedValue;
    }
    
    /// <summary>
    /// Safely retrieves a typed value from a Pulumi resource mock input based on a strongly typed input property selector.
    /// Returns the default value if the input is missing or the value cannot be cast to the expected type.
    /// </summary>
    /// <typeparam name="TResource">
    /// The Pulumi <see cref="ResourceArgs"/> type that defines the resource inputs (e.g., <c>ResourceGroupArgs</c>).
    /// </typeparam>
    /// <typeparam name="TValue">The expected type of the value to retrieve.</typeparam>
    /// <param name="inputs">The list of mocked inputs for the Pulumi test environment.</param>
    /// <param name="logicalName">The logical name (ID) of the resource to match in the input list.</param>
    /// <param name="propertySelector">
    /// An expression selecting the desired input property (e.g., <c>x => x.Location</c>).
    /// The property name is automatically converted to camelCase to match Pulumi’s input schema.
    /// </param>
    /// <returns>
    /// The value of type <typeparamref name="TValue"/> if found and castable; otherwise, the default value for <typeparamref name="TValue"/>.
    /// </returns>
    public static TValue? GetValue<TResource, TValue>(this ImmutableList<Input> inputs, 
        string logicalName, Expression<Func<TResource, object?>> propertySelector)
        where TResource : ResourceArgs
    {
        string key = propertySelector.GetInputName();

        Input? input = inputs.SingleOrDefault(x => x.Id.Equals(logicalName, StringComparison.Ordinal));
        if (input is null 
            || !input.Inputs.TryGetValue(key, out object? value) 
            || value is not TValue typedValue)
        {
            return default;
        }

        return typedValue;
    }
}
