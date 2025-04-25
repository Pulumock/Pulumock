using System.Linq.Expressions;
using Pulumock.Extensions;

namespace Pulumock.Mocks.Builders;

public class NestedOutputsBuilder<T>
{
    private readonly Dictionary<string, object> _values = new();

    public NestedOutputsBuilder<T> WithNestedOutput(Expression<Func<T, object>> selector, object value)
    {
        _values[selector.GetOutputName()] = value;
        return this;
    }

    public Dictionary<string, object> Build() => _values;
}
