using System.Collections.Immutable;
using Pulumock.Mocks.Models;

namespace Pulumock.Mocks.Builders;

/// <summary>
/// A fluent builder for creating a <see cref="MockCall"/>.
/// </summary>
public class MockCallBuilder
{
    private readonly Dictionary<string, object> _outputs = new();

    /// <summary>
    /// Adds a mocked output property for the function call result.
    /// </summary>
    /// <param name="key">The output property name.</param>
    /// <param name="value">The mocked return value for the property.</param>
    public MockCallBuilder WithOutput(string key, object value)
    {
        _outputs.Add(key, value);
        return this;
    }

    /// <summary>
    /// Builds the <see cref="MockCall"/> instance with the specified function type and mocked outputs.
    /// <param name="type">The provider function <see cref="Type"/>.</param>
    /// </summary>
    public MockCall Build(Type type) =>
        new(type, _outputs.ToImmutableDictionary());
}
