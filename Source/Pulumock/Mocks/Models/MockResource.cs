using System.Collections.Immutable;

namespace Pulumock.Mocks.Models;

/// <summary>
/// Represents a mocked Pulumi resource.
/// </summary>
/// <param name="Type">
/// The .NET <see cref="Type"/> that represents the Pulumi resource being mocked.
/// </param>
/// <param name="MockOutputs">
/// A dictionary of mocked output values that the resource is expected to expose during testing.
/// Keys should match the output property names defined on the real Pulumi resource type.
/// </param>
public record MockResource(Type Type, ImmutableDictionary<string, object> MockOutputs, string? LogicalName = null);
