using System.Linq.Expressions;
using Pulumock.Extensions;

namespace Pulumock.Mocks.Builders;

/// <summary>
/// A builder for nested output structures for mocked resources or calls.
/// </summary>
/// <typeparam name="T">The type representing the nested output structure.</typeparam>
public class NestedOutputsBuilder<T>
{
    private readonly Dictionary<string, object?> _values = new();

    /// <summary>
    /// Adds a nested output value using a strongly-typed property selector.
    /// </summary>
    /// <param name="selector">An expression selecting the nested output property.</param>
    /// <param name="value">The mocked value to assign to the nested property.</param>
    public NestedOutputsBuilder<T> WithNestedOutput(Expression<Func<T, object?>> selector, object? value)
    {
        _values[selector.GetOutputName()] = value;
        return this;
    }

    /// <summary>
    /// Builds the nested output dictionary to be used in a mock.
    /// </summary>
    /// <returns>A dictionary representing the nested outputs.</returns>
    public Dictionary<string, object?> Build() => _values;
}
