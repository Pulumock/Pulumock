using System.Collections.Immutable;
using System.Linq.Expressions;
using Pulumi;
using Pulumock.Extensions;
using Pulumock.Mocks.Models;

namespace Pulumock.Mocks.Builders;

/// <summary>
/// A fluent builder for creating a <see cref="MockResource"/>.
/// </summary>
public class MockResourceBuilder<T>(string? logicalName = null)
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
    
    public MockResourceBuilder<T> WithOutput(Expression<Func<T, object?>> propertySelector, object value)
    {
        _outputs.Add(propertySelector.GetOutputName(), value);
        return this;
    }
    
    /// <summary>
    /// Adds a mocked output key and value to the resource using a strongly typed property selector.
    /// </summary>
    /// <param name="propertySelector">
    /// An expression selecting the <see cref="Output{T}"/> property on the resource to mock.
    /// This should point to a property decorated with Pulumi's <see cref="OutputAttribute"/>.
    /// </param>
    /// <param name="value">The mocked value.</param>
    public MockResourceBuilder<T> WithOutput<TProperty>(Expression<Func<TProperty, object?>> propertySelector, object value)
    {
        _outputs.Add(propertySelector.GetOutputName(), value);
        return this;
    }
    
    public MockResourceBuilder<T> WithOutput<TNested, TNestedValue>(
        Expression<Func<T, object?>> propertySelector,
        Expression<Func<TNested, object?>> nestedPropertySelector,
        TNestedValue value)
    {
        NestedOutputsBuilder<TNested> builder = new NestedOutputsBuilder<TNested>()
            .WithNestedOutput(nestedPropertySelector, value);

        _outputs.Add(propertySelector.GetOutputName(), builder.Build());
        return this;
    }
    
    /// <summary>
    /// Builds the <see cref="MockResource"/> mock.
    /// </summary>
    public MockResource Build() => 
        new(typeof(T), _outputs.ToImmutableDictionary(), logicalName);
}
