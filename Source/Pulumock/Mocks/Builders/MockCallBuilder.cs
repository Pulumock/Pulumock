using System.Collections.Immutable;
using System.Linq.Expressions;
using Pulumock.Extensions;
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
    /// Adds a mocked output property for the function call result.
    /// </summary>
    /// <typeparam name="T">The function result type.</typeparam>
    /// <param name="propertySelector">A lambda expression selecting the output property.</param>
    /// <param name="value">The mocked return value for the property.</param>
    public MockCallBuilder WithOutput<T>(Expression<Func<T, object>> propertySelector, object value)
    {
        _outputs.Add(propertySelector.GetOutputName(), value);
        return this;
    }

    /// <summary>
    /// Builds the <see cref="MockCall"/> instance with the specified function type and mocked outputs.
    /// <param name="type">The provider function <see cref="Type"/>.</param>
    /// </summary>
    public MockCall Build(Type type) =>
        new(MockCallToken.FromTypeToken(type), _outputs.ToImmutableDictionary());
    
    /// <summary>
    /// Builds the <see cref="MockCall"/> instance with the specified function token and mocked outputs.
    /// <param name="token">The provider function token identifier.</param>
    /// </summary>
    public MockCall Build(string token) =>
        new(MockCallToken.FromStringToken(token), _outputs.ToImmutableDictionary());
}
