using System.Collections.Immutable;

namespace Pulumock.Mocks.Models;

public sealed record EnrichedCall(string Token, ImmutableDictionary<string, object> Inputs, ImmutableDictionary<string, object> Outputs);
