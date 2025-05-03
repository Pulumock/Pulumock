using System.Collections.Immutable;
using Pulumi;
using Pulumi.Testing;
using Pulumock.Mocks.Models;
using Pulumock.Utilities;

namespace Pulumock.Mocks;

/// <summary>
/// Provides an implementation of <see cref="Pulumi.Testing.IMocks"/> for unit testing Pulumi stacks.
/// This class is responsible for mocking both resource creation and provider function (invoke) calls.
/// </summary>
internal sealed class Mocks(ImmutableDictionary<(Type Type, string? LogicalName), MockResource> mockResources, 
    ImmutableDictionary<MockCallToken, MockCall> mockCalls) : IMocks
{
    private readonly List<EnrichedResource> _enrichedResources = [];
    private readonly List<EnrichedCall> _enrichedCalls = [];
    
    public Task<(string? id, object state)> NewResourceAsync(MockResourceArgs args)
    {
        ImmutableDictionary<string, object>.Builder outputs = ImmutableDictionary.CreateBuilder<string, object>();
        string logicalResourceName = MockResourceHelper.GetLogicalName(args.Name);
        string resourceId = MockResourceHelper.GetId(args.Id, $"{logicalResourceName}_id");
        
        if (MockResourceHelper.IsStackReference(args))
        {
            if (mockResources.TryGetValue((typeof(StackReference), logicalResourceName), out MockResource? mockResource))
            {
                outputs.Add("outputs", mockResource.MockOutputs);
                outputs.Add("secretOutputNames", ImmutableArray<string>.Empty);
            }
        }
        else
        {
            MockResource? mockResource = MockResourceHelper.GetOrDefault(mockResources, args.Type, args.Name);
            if (mockResource is not null)
            {
                outputs.AddRange(mockResource.MockOutputs);
            }
            
            outputs.Add("name", MockResourceHelper.GetPhysicalName(args, outputs));
        }
        
        _enrichedResources.Add(new EnrichedResource(args.Type, logicalResourceName, args.Inputs));
        
        ImmutableDictionary<string, object> mergedOutputs = OutputMerger.Merge(args.Inputs, outputs);
        
        return Task.FromResult<(string?, object)>((resourceId, mergedOutputs));
    }
    
    public Task<object> CallAsync(MockCallArgs args)
    {
        string callToken = MockCallHelper.GetToken(args.Token);
        
        ImmutableDictionary<string, object>.Builder outputs = ImmutableDictionary.CreateBuilder<string, object>();
        
        MockCall? mockCall = MockCallHelper.GetOrDefault(mockCalls, callToken);
        if (mockCall is not null)
        {
            outputs.AddRange(mockCall.MockOutputs);
        }

        ImmutableDictionary<string, object> mergedOutputs = OutputMerger.Merge(args.Args, outputs);
        
        _enrichedCalls.Add(new EnrichedCall(callToken, args.Args, mergedOutputs));
        
        return Task.FromResult<object>(mergedOutputs);
    }
    
    /// <summary>
    /// Gets the list of enriched Pulumi resources captured during the test run, including their logical names and raw inputs.
    /// </summary>
    public ImmutableList<EnrichedResource> EnrichedResources => _enrichedResources.ToImmutableList();
    
    /// <summary>
    /// Gets the list of enriched Pulumi function calls captured during the test run, including their tokens, inputs, and outputs.
    /// </summary>
    public ImmutableList<EnrichedCall> EnrichedCalls => _enrichedCalls.ToImmutableList();
}
