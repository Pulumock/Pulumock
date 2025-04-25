using System.Collections.Immutable;
using System.Linq.Expressions;
using Pulumi;
using Pulumock.Extensions;
using Pulumock.Mocks.Models;

namespace Pulumock.Mocks.Builders;

/// <summary>
/// A fluent builder for creating a <see cref="MockResource"/>.
/// </summary>
public class MockResourceBuilder
{
    private readonly Dictionary<string, object> _outputs = new();

    /// <summary>
    /// Adds a mocked output key and value to the resource.
    /// </summary>
    /// <param name="key">The output property name.</param>
    /// <param name="value">The mocked value.</param>
    public MockResourceBuilder WithOutput(string key, object value)
    {
        _outputs.Add(key, value);
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
    public MockResourceBuilder WithOutput<T>(Expression<Func<T, object>> propertySelector, object value)
    {
        _outputs.Add(propertySelector.GetOutputName(), value);
        return this;
    }
    
    public MockResourceBuilder WithOutput<T, TNested>(
        Expression<Func<T, object>> propertySelector,
        Func<NestedOutputsBuilder<TNested>, NestedOutputsBuilder<TNested>> nestedOutputsBuilder)
    {
        string topLevelOutputName = propertySelector.GetOutputName();

        Dictionary<string, object> nestedOutputs = nestedOutputsBuilder(new NestedOutputsBuilder<TNested>())
            .Build();
        
        _outputs[topLevelOutputName] = nestedOutputs;
        return this;
    }
    
    /// <summary>
    /// Builds the <see cref="MockResource"/> mock.
    /// </summary>
    public MockResource Build<T>(string? logicalName = null) => 
        new(typeof(T), _outputs.ToImmutableDictionary(), logicalName);
}
