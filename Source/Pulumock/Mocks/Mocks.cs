using System.Collections.Immutable;
using Pulumi.Testing;
using Pulumock.Extensions;
using Pulumock.Mocks.Constants;
using Pulumock.Mocks.Models;

namespace Pulumock.Mocks;

// TODO: full upsert, partial upsert | (with/without methods for all) 
// TODO: support both typed and non-typed builders and with() methods

/// <summary>
/// Provides an implementation of <see cref="Pulumi.Testing.IMocks"/> for unit testing Pulumi stacks.
/// This class is responsible for mocking both resource creation and provider function (invoke) calls
/// so that Pulumi programs can be tested without deploying actual cloud infrastructure.
/// </summary>
internal sealed class Mocks(ImmutableDictionary<(Type Type, string? LogicalName), MockResource> mockResources, 
    ImmutableDictionary<MockCallToken, MockCall> mockCalls) : IMocks
{
    private readonly List<ResourceSnapshot> _resourceSnapshots = [];
    private readonly List<CallSnapshot> _callSnapshots = [];
    
    public ImmutableList<ResourceSnapshot> ResourceSnapshots => _resourceSnapshots.ToImmutableList();
    public ImmutableList<CallSnapshot> CallSnapshots => _callSnapshots.ToImmutableList();
    
    public Task<(string? id, object state)> NewResourceAsync(MockResourceArgs args)
    {
        ImmutableDictionary<string, object>.Builder outputs = ImmutableDictionary.CreateBuilder<string, object>();
        outputs.AddRange(args.Inputs);
        
        if (string.Equals(args.Type, ResourceTypeTokenConstants.StackReference, StringComparison.Ordinal))
        {
            if (mockResources.TryGetValue((typeof(MockStackReference), GetLogicalResourceName(args.Name)), out MockResource? mockResource))
            {
                outputs.Add("outputs", mockResource.MockOutputs);
            }
            
            outputs.Add("secretOutputNames", ImmutableArray<string>.Empty);
        }
        else
        {
            MockResource? mockResource = mockResources
                .SingleOrDefault(kvp =>
                    kvp.Key.Type.MatchesResourceTypeToken(args.Type) &&
                    string.Equals(kvp.Key.LogicalName, args.Name, StringComparison.Ordinal))
                .Value;
            
            mockResource ??= mockResources
                .SingleOrDefault(kvp =>
                    kvp.Key.Type.MatchesResourceTypeToken(args.Type) &&
                    kvp.Key.LogicalName is null)
                .Value;

            if (mockResource is not null)
            {
                outputs.AddRange(mockResource.MockOutputs);
            }
            
            object physicalResourceName = outputs.GetValueOrDefault("name") ?? $"{GetLogicalResourceName(args.Name)}_physical";
            outputs.Add("name", physicalResourceName);
        }
        
        string resourceName = GetLogicalResourceName(args.Name);
        string resourceId = GetResourceId(args.Id, $"{resourceName}_id");
        
        _resourceSnapshots.Add(new ResourceSnapshot(resourceName, args.Inputs));
        return Task.FromResult<(string?, object)>((resourceId, outputs.ToImmutable()));
    }

    public Task<object> CallAsync(MockCallArgs args)
    {
        string callToken = GetCallToken(args.Token);
        
        ImmutableDictionary<string, object>.Builder outputs = ImmutableDictionary.CreateBuilder<string, object>();
        outputs.AddRange(args.Args);
        
        MockCall? mockCall = mockCalls
            .FirstOrDefault(kvp =>
                kvp.Key.IsStringToken &&
                string.Equals(kvp.Key.StringTokenValue, callToken, StringComparison.Ordinal))
            .Value;

        mockCall ??= mockCalls
            .FirstOrDefault(kvp =>
                kvp.Key.IsTypeToken &&
                kvp.Key.TypeTokenValue.MatchesCallTypeToken(callToken))
            .Value;

        if (mockCall is not null)
        {
            outputs.AddRange(mockCall.MockOutputs);
        }
        
        ImmutableDictionary<string, object> finalOutputs = outputs.ToImmutable();
        
        _callSnapshots.Add(new CallSnapshot(callToken, args.Args, finalOutputs));
        
        return Task.FromResult<object>(finalOutputs);
    }
    
    private static string GetLogicalResourceName(string? name) =>
        string.IsNullOrWhiteSpace(name) ? throw new ArgumentNullException(nameof(name)) : name;
    
    private static string GetResourceId(string? id, string fallbackId) =>
        string.IsNullOrWhiteSpace(id) ? fallbackId : id;
    
    private static string GetCallToken(string? token) =>
        string.IsNullOrWhiteSpace(token) ? throw new ArgumentNullException(nameof(token)) : token;
}
