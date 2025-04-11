using System.Collections.Immutable;
using Pulumock.Mocks.Models;

namespace Pulumock.Mocks.Builders;

/// <summary>
/// A fluent builder for creating a <see cref="MockStackReference"/>.
/// </summary>
/// <param name="fullyQualifiedStackName">
/// The fully qualified name of the referenced stack, typically in the format <c>org/project/stack</c>.
/// </param>
public class MockStackReferenceBuilder(string fullyQualifiedStackName)
{
    private readonly Dictionary<string, object> _outputs = new();
    
    /// <summary>
    /// Adds a mocked output key and value to the stack reference.
    /// </summary>
    /// <param name="key">The output property name.</param>
    /// <param name="value">The mocked value.</param>
    public MockStackReferenceBuilder WithOutput(string key, object value)
    {
        _outputs.Add(key, value);
        return this;
    }

    /// <summary>
    /// Builds the <see cref="MockStackReference"/> mock.
    /// </summary>
    public MockStackReference Build() =>
        new(fullyQualifiedStackName, _outputs.ToImmutableDictionary());
}
