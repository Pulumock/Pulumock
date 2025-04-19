using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection;
using Pulumi;

namespace Pulumock.Extensions;

public static class ResourceExtensions
{
    public static T GetResourceByLogicalName<T>(this ImmutableArray<Resource> resources, string logicalName) where T : Resource =>
        resources
            .OfType<T>()
            .Single(x => x.GetResourceName().Equals(logicalName, StringComparison.Ordinal));
    
    public static string GetResourceOutputName<T>(this Expression<Func<T, object>> propertySelector)
    {
        MemberInfo member = propertySelector.Body switch
        {
            MemberExpression m => m.Member,
            UnaryExpression { Operand: MemberExpression m } => m.Member,
            _ => throw new ArgumentException("Invalid property selector")
        };

        return member.GetCustomAttribute<OutputAttribute>()?.Name ?? member.Name;
    }
}
