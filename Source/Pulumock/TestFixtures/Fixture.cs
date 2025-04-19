using System.Collections.Immutable;
using Pulumi;

namespace Pulumock.TestFixtures;

public record Fixture(ImmutableArray<Resource> StackResources, ImmutableDictionary<string, object?> StackOutputs);
