using System.Collections.Immutable;
using Pulumi;

namespace Pulumock.Extensions;

public static class ResourceExtensions
{
    public static T GetResourceByLogicalName<T>(this ImmutableArray<Resource> resources, string logicalName) where T : Resource =>
        resources
            .OfType<T>()
            .Single(x => x.GetResourceName().Equals(logicalName, StringComparison.Ordinal));
}
