using System.Collections.Immutable;

namespace Pulumock.Mocks.Models;

public record Input(string Id, ImmutableDictionary<string, object> Inputs);
