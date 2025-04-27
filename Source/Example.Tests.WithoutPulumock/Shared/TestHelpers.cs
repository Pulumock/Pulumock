using System.Reflection;
using Pulumi;

namespace Example.Tests.WithoutPulumock.Shared;

internal static class TestHelpers
{
    public static bool IsChildOf(Resource child, Resource potentialParent)
    {
        PropertyInfo? childResourcesField = typeof(Resource)
            .GetProperty("ChildResources", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        if (childResourcesField is null)
        {
            throw new InvalidOperationException("Could not reflect ChildResources field.");
        }

        var children = childResourcesField.GetValue(potentialParent) as IEnumerable<Resource>;

        return children?.Contains(child) ?? false;
    }
}
