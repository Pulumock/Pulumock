using System.Collections.Immutable;
using Pulumi;
using Pulumock.Mocks.Models;

namespace Pulumock.TestFixtures;

// TODO: rename CallSnapshots to EnrichedCalls
public record Fixture(ImmutableArray<Resource> StackResources, 
    ImmutableDictionary<string, object?> StackOutputs, 
    ImmutableList<EnrichedResource> EnrichedStackResources,
    ImmutableList<CallSnapshot> CallSnapshots);
