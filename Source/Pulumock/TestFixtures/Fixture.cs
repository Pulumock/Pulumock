using System.Collections.Immutable;
using Pulumi;
using Pulumock.Mocks.Models;

namespace Pulumock.TestFixtures;

// TODO: rename ResourceSnapshots and CallSnapshots to EnrichedStackResources and EnrichedCalls
public record Fixture(ImmutableArray<Resource> StackResources, 
    ImmutableDictionary<string, object?> StackOutputs, 
    ImmutableList<ResourceSnapshot> ResourceSnapshots,
    ImmutableList<CallSnapshot> CallSnapshots);
