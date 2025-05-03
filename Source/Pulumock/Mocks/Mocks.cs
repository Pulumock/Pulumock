using System.Collections.Immutable;
using Pulumi;
using Pulumi.Testing;
using Pulumock.Mocks.Models;
using Pulumock.Utilities;

namespace Pulumock.Mocks;

/// <summary>
/// Provides an implementation of <see cref="Pulumi.Testing.IMocks"/> for unit testing Pulumi stacks.
/// This class is responsible for mocking both resource creation and provider function (invoke) calls
/// so that Pulumi programs can be tested without deploying actual cloud infrastructure.
/// </summary>
internal sealed class Mocks(ImmutableDictionary<(Type Type, string? LogicalName), MockResource> mockResources, 
    ImmutableDictionary<MockCallToken, MockCall> mockCalls) : IMocks
{
    private readonly List<EnrichedResource> _enrichedResources = [];
    private readonly List<CallSnapshot> _callSnapshots = [];
    
    public Task<(string? id, object state)> NewResourceAsync(MockResourceArgs args)
    {
        ImmutableDictionary<string, object>.Builder outputs = ImmutableDictionary.CreateBuilder<string, object>();
        string logicalResourceName = MockHelper.GetLogicalResourceName(args.Name);
        string resourceId = MockHelper.GetResourceId(args.Id, $"{logicalResourceName}_id");
        
        if (MockHelper.IsStackReference(args))
        {
            if (mockResources.TryGetValue((typeof(StackReference), logicalResourceName), out MockResource? mockResource))
            {
                outputs.Add("outputs", mockResource.MockOutputs);
                outputs.Add("secretOutputNames", ImmutableArray<string>.Empty);
            }
        }
        else
        {
            MockResource? mockResource = MockHelper.GetMockResourceOrDefault(mockResources, args.Type, args.Name);
            if (mockResource is not null)
            {
                outputs.AddRange(mockResource.MockOutputs);
            }
            
            outputs.Add("name", MockHelper.GetPhysicalResourceName(args, outputs));
        }
        
        _enrichedResources.Add(new EnrichedResource(args.Type, logicalResourceName, args.Inputs));
        
        ImmutableDictionary<string, object> mergedOutputs = OutputMerger.Merge(args.Inputs, outputs);
        
        return Task.FromResult<(string?, object)>((resourceId, mergedOutputs));
    }
    
    public Task<object> CallAsync(MockCallArgs args)
    {
        string callToken = MockHelper.GetCallToken(args.Token);
        
        ImmutableDictionary<string, object>.Builder outputs = ImmutableDictionary.CreateBuilder<string, object>();
        
        MockCall? mockCall = MockHelper.GetMockCallOrDefault(mockCalls, callToken);
        if (mockCall is not null)
        {
            outputs.AddRange(mockCall.MockOutputs);
        }

        ImmutableDictionary<string, object> mergedOutputs = OutputMerger.Merge(args.Args, outputs);
        
        _callSnapshots.Add(new CallSnapshot(callToken, args.Args, mergedOutputs));
        
        return Task.FromResult<object>(mergedOutputs);
    }
    
    public ImmutableList<EnrichedResource> EnrichedResources => _enrichedResources.ToImmutableList();
    public ImmutableList<CallSnapshot> CallSnapshots => _callSnapshots.ToImmutableList();
}
