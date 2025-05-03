using System.Collections.Immutable;

namespace Pulumock.Mocks.Models;

public sealed record EnrichedResource(string? TypeToken, string LogicalName, ImmutableDictionary<string, object> Inputs);
