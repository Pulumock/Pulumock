using System.Collections.Immutable;
using Pulumi.Testing;
using Pulumock.Extensions;
using Pulumock.Mocks.Constants;
using Pulumock.Mocks.Models;

namespace Pulumock.Utilities;

/// <summary>
/// Provides utility methods for <see cref="MockResource"/>.
/// </summary>
public static class MockResourceHelper
{
    /// <summary>
    /// Determines whether the given mock resource represents a Pulumi <c>StackReference</c>.
    /// </summary>
    /// <param name="args">The resource arguments to inspect.</param>
    /// <returns><c>true</c> if the resource is a stack reference; otherwise, <c>false</c>.</returns>
    public static bool IsStackReference(MockResourceArgs args) => 
        string.Equals(args.Type, ResourceTypeTokenConstants.StackReference, StringComparison.Ordinal);

    /// <summary>
    /// Ensures the provided logical name is not null or whitespace.
    /// </summary>
    /// <param name="name">The logical name of the resource.</param>
    /// <returns>The validated logical name.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the name is null or empty.</exception>
    public static string GetLogicalName(string? name) =>
        string.IsNullOrWhiteSpace(name) ? throw new ArgumentNullException(nameof(name)) : name;
    
    /// <summary>
    /// Resolves the physical name for a mocked resource.
    /// Falls back to a default naming pattern if none is defined in the outputs.
    /// </summary>
    /// <param name="args">The resource arguments.</param>
    /// <param name="outputs">The mocked output dictionary.</param>
    /// <returns>The resolved physical name.</returns>
    public static object GetPhysicalName(MockResourceArgs args, ImmutableDictionary<string, object>.Builder outputs) => 
        outputs.GetValueOrDefault("name") ?? $"{GetLogicalName(args.Name)}_physical";

    /// <summary>
    /// Returns the given ID if it is valid, otherwise falls back to a default ID.
    /// </summary>
    /// <param name="id">The potentially provided ID.</param>
    /// <param name="fallbackId">The fallback value if <paramref name="id"/> is null or whitespace.</param>
    /// <returns>The resolved ID.</returns>
    public static string GetId(string? id, string fallbackId) =>
        string.IsNullOrWhiteSpace(id) ? fallbackId : id;
    
    /// <summary>
    /// Attempts to retrieve a matching mocked resource based on its type token and optional logical name.
    /// </summary>
    /// <param name="resources">The dictionary of registered mock resources.</param>
    /// <param name="typeToken">The resource type token to match.</param>
    /// <param name="logicalName">The optional logical name to match.</param>
    /// <returns>The matching <see cref="MockResource"/> if found, otherwise <c>null</c>.</returns>
    public static MockResource? GetOrDefault(
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
}
