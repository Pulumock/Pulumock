using System.Collections.Immutable;

namespace Pulumock.Mocks.Models;

/// <summary>
/// Represents a mocked <see href="https://www.pulumi.com/docs/iac/concepts/resources/functions/">Pulumi provider function</see>
/// </summary>
/// <param name="MockOutputs">
/// A dictionary of mocked output values that the function is expected to return during testing.
/// These keys should match the output property names defined by the real Pulumi function.
/// </param>
public record MockCall(MockCallToken Token, ImmutableDictionary<string, object> MockOutputs);
