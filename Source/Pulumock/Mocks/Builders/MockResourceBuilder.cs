using System.Collections.Immutable;
using System.Linq.Expressions;
using Pulumi;
using Pulumock.Extensions;
using Pulumock.Mocks.Models;

namespace Pulumock.Mocks.Builders;

/// <summary>
/// A fluent builder for creating a <see cref="MockResource"/>.
/// </summary>
/// <typeparam name="T">The Pulumi resource type this mock represents.</typeparam>
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
        Func<OutputPathBuilder<TNested>, OutputPathBuilder<TNested>> nestedBuilder)
    {
        string[] path = propertySelector.GetOutputPath();

        Dictionary<string, object> nestedValues = nestedBuilder(new OutputPathBuilder<TNested>()).Build();

        SetNestedValue(_outputs, path, nestedValues);

        return this;
    }

    /// <summary>
    /// Builds the <see cref="MockResource"/> mock.
    /// </summary>
    public MockResource Build<T>(string? logicalName = null) => 
        new(typeof(T), _outputs.ToImmutableDictionary(), logicalName);
    
    #pragma warning disable CA1859
    private static void SetNestedValue(IDictionary<string, object> dict, string[] path, object value)
    #pragma warning restore CA1859
    {
        for (int i = 0; i < path.Length - 1; i++)
        {
            string key = path[i];

            if (!dict.TryGetValue(key, out object? next) || next is not IDictionary<string, object> nextDict)
            {
                nextDict = new Dictionary<string, object>();
                dict[key] = nextDict;
            }

            dict = nextDict;
        }

        dict[path[^1]] = value;
    }
}

public class OutputPathBuilder<T>
{
    private readonly Dictionary<string, object> _values = new();

    public OutputPathBuilder<T> Prop<TNested>(Expression<Func<TNested, object>> selector, object value)
    {
        string key = selector.GetOutputName();
        _values[key] = value;
        return this;
    }

    public Dictionary<string, object> Build() => _values;
}
