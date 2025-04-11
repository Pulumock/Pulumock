using System.Collections.Immutable;
using Pulumock.Mocks.Models;

namespace Pulumock.Mocks.Builders;

/// <summary>
/// A fluent builder for creating a <see cref="MockCall"/>.
/// </summary>
/// <typeparam name="T">The Pulumi invoke function type to be mocked.</typeparam>
public class MockCallBuilder<T>
{
    private readonly Dictionary<string, object> _outputs = new();

    /// <summary>
    /// Adds a mocked output property for the function call result.
    /// </summary>
    /// <param name="key">The output property name.</param>
    /// <param name="value">The mocked return value for the property.</param>
    public MockCallBuilder<T> WithOutput(string key, object value)
    {
        _outputs.Add(key, value);
        return this;
    }

    /// <summary>
    /// Builds the <see cref="MockCall"/> instance with the specified function type and mocked outputs.
    /// </summary>
    public MockCall Build() =>
        new(typeof(T), _outputs.ToImmutableDictionary());
}
