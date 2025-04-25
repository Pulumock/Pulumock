using System.Collections.Immutable;
using Pulumi;
using Pulumock.Mocks.Models;

namespace Pulumock.TestFixtures;

public record Fixture(ImmutableArray<Resource> StackResources, 
    ImmutableDictionary<string, object?> StackOutputs, 
    ImmutableList<ResourceSnapshot> ResourceSnapshots,
    ImmutableList<CallSnapshot> CallSnapshots);
