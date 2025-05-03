using System.Collections.Immutable;

namespace Pulumock.Mocks.Models;

/// <summary>
/// Represents a mocked Pulumi resource, including its type, mocked outputs, and optional logical name.
/// </summary>
/// <param name="Type">
/// The .NET <see cref="Type"/> representing the Pulumi resource being mocked.
/// </param>
/// <param name="MockOutputs">
/// A dictionary of mocked output values that the resource is expected to expose during testing.
/// Keys must match the output property names defined on the resource type.
/// </param>
/// <param name="LogicalName">
/// An optional logical name used to distinguish specific resource instances in tests (e.g., for name-based matching).
/// </param>
public record MockResource(Type Type, ImmutableDictionary<string, object> MockOutputs, string? LogicalName = null);
