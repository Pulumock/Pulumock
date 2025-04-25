using System.Collections.Immutable;
using Pulumi;
using Pulumi.Testing;
using Pulumock.Extensions;
using Pulumock.Mocks.Constants;
using Pulumock.Mocks.Models;

namespace Pulumock.Utilities;

public static class MockHelper
{
    public static bool IsStackReference(MockResourceArgs args) => 
        string.Equals(args.Type, ResourceTypeTokenConstants.StackReference, StringComparison.Ordinal);

    public static string GetLogicalResourceName(string? name) =>
        string.IsNullOrWhiteSpace(name) ? throw new ArgumentNullException(nameof(name)) : name;
    
    public static object GetPhysicalResourceName(MockResourceArgs args, ImmutableDictionary<string, object>.Builder outputs) => 
        outputs.GetValueOrDefault("name") ?? $"{GetLogicalResourceName(args.Name)}_physical";

    public static string GetResourceId(string? id, string fallbackId) =>
        string.IsNullOrWhiteSpace(id) ? fallbackId : id;
    
    public static string GetCallToken(string? token) =>
        string.IsNullOrWhiteSpace(token) ? throw new ArgumentNullException(nameof(token)) : token;
    
    public static MockResource? GetMockResourceOrDefault(
        ImmutableDictionary<(Type Type, string? LogicalName), MockResource> resources,
        string? typeToken,
        string? logicalName)
    {
        MockResource? match = resources
            .SingleOrDefault(kvp =>
                kvp.Key.Type.MatchesResourceTypeToken(typeToken) &&
                string.Equals(kvp.Key.LogicalName, logicalName, StringComparison.Ordinal))
            .Value;

        return match ?? resources
            .SingleOrDefault(kvp =>
                kvp.Key.Type.MatchesResourceTypeToken(typeToken) &&
                kvp.Key.LogicalName is null)
            .Value;
    }
    
    public static MockCall? GetMockCallOrDefault(ImmutableDictionary<MockCallToken, MockCall> calls, string callToken)
    {
        MockCall? match = calls
            .FirstOrDefault(kvp =>
                kvp.Key.IsStringToken &&
                string.Equals(kvp.Key.StringTokenValue, callToken, StringComparison.Ordinal))
            .Value;
        
        return match ?? calls
            .FirstOrDefault(kvp =>
                kvp.Key.IsTypeToken &&
                kvp.Key.TypeTokenValue.MatchesCallTypeToken(callToken))
            .Value;
    }
}
