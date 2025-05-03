using System.Collections.Immutable;
using Pulumi;
using Pulumock.Mocks.Models;

namespace Pulumock.TestFixtures;

/// <summary>
/// Represents the result of a Pulumock test execution, containing all captured resources,
/// stack outputs, and enriched test metadata for inspection and assertions.
/// </summary>
/// <param name="StackResources">
/// The Pulumi resources created during the test run, including both component and custom resources.
/// </param>
/// <param name="StackOutputs">
/// The outputs exposed by the Pulumi stack under test.
/// </param>
/// <param name="EnrichedStackResources">
/// Enriched metadata for created resources, including their logical names and raw input arguments.
/// </param>
/// <param name="EnrichedStackCalls">
/// Enriched metadata for all provider function calls made by the stack, including tokens, inputs, and mocked outputs.
/// </param>
public record Fixture(ImmutableArray<Resource> StackResources, 
    ImmutableDictionary<string, object?> StackOutputs, 
    ImmutableList<EnrichedResource> EnrichedStackResources,
    ImmutableList<EnrichedCall> EnrichedStackCalls);
