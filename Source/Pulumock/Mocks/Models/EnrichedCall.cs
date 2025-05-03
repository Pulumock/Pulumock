using System.Collections.Immutable;

namespace Pulumock.Mocks.Models;

/// <summary>
/// Represents an intercepted Pulumi function call enriched with its token, raw inputs, and outputs.
/// </summary>
/// <param name="Token">The fully qualified function call token (e.g., <c>"azure-native:authorization:getRoleDefinition"</c>).</param>
/// <param name="Inputs">The raw input arguments passed to the function call.</param>
/// <param name="Outputs">The mocked or resolved output values returned from the function call.</param>
public sealed record EnrichedCall(string Token, ImmutableDictionary<string, object> Inputs, ImmutableDictionary<string, object> Outputs);
