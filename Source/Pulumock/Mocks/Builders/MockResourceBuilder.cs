using System.Collections.Immutable;
using Pulumock.Mocks.Models;

namespace Pulumock.Mocks.Builders;

/// <summary>
/// A fluent builder for creating a <see cref="MockResource"/>.
/// </summary>
/// <typeparam name="T">The Pulumi resource type this mock represents.</typeparam>
public class MockResourceBuilder<T>
{
    private readonly Dictionary<string, object> _outputs = new();

    /// <summary>
    /// Adds a mocked output key and value to the resource.
    /// </summary>
    /// <param name="key">The output property name.</param>
    /// <param name="value">The mocked value.</param>
    public MockResourceBuilder<T> WithOutput(string key, object value)
    {
        _outputs.Add(key, value);
        return this;
    }

    /// <summary>
    /// Builds the <see cref="MockResource"/> mock.
    /// </summary>
    public MockResource Build() => 
        new(typeof(T), _outputs.ToImmutableDictionary());
}
