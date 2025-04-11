using System.Collections.Immutable;
using Pulumi;

namespace Pulumock.Mocks.Models;

/// <summary>
/// Represents a mocked <see cref="Pulumi.StackReference"/>.
/// </summary>
/// <param name="FullyQualifiedStackName">
/// The fully qualified name of the referenced stack, typically in the format <c>org/project/stack</c>.
/// </param>
/// <param name="MockOutputs">
/// A dictionary representing the mocked outputs returned by the referenced stack.
/// </param>
public record MockStackReference(string FullyQualifiedStackName, ImmutableDictionary<string, object> MockOutputs)
    : MockResource(typeof(StackReference), MockOutputs);
