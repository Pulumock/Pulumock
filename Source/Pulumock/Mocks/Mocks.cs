using System.Collections.Immutable;
using Pulumi.Testing;
using Pulumock.Extensions;
using Pulumock.Mocks.Constants;
using Pulumock.Mocks.Models;

namespace Pulumock.Mocks;

/// <summary>
/// Provides an implementation of <see cref="Pulumi.Testing.IMocks"/> for unit testing Pulumi stacks.
/// This class is responsible for mocking both resource creation and provider function (invoke) calls
/// so that Pulumi programs can be tested without deploying actual cloud infrastructure.
/// </summary>
internal sealed class Mocks(IReadOnlyCollection<MockResource> mockResources, IReadOnlyCollection<MockCall> mockCalls) : IMocks
{
    private readonly List<Input> _inputs = [];
    public ImmutableList<Input> Inputs => _inputs.ToImmutableList();
    
    public Task<(string? id, object state)> NewResourceAsync(MockResourceArgs args)
    {
        ImmutableDictionary<string, object>.Builder outputs = ImmutableDictionary.CreateBuilder<string, object>();
        
        if (string.Equals(args.Type, ResourceTypeTokenConstants.StackReference, StringComparison.Ordinal))
        {
            // TODO: what if more than one ref is defined (merge somehow so we can append new config without having to pass the entire object again?)
            ImmutableDictionary<string, object> mockOutputs = mockResources
                .OfType<MockStackReference>()
                .Single(stackReferences => string.Equals(stackReferences.FullyQualifiedStackName, args.Name, StringComparison.Ordinal))
                .MockOutputs;
            
            outputs.Add("outputs", mockOutputs);
            outputs.Add("secretOutputNames", ImmutableArray<string>.Empty);
        }
        else
        {
            // TODO: enable mocking specifically by logical name
            IEnumerable<ImmutableDictionary<string, object>> resourceMockOutputs = mockResources
                .Where(mockResource => mockResource.Type.MatchesResourceTypeToken(args.Type))
                .Select(mockResource => mockResource.MockOutputs);
            
            // TODO: don't add all, select latest added (or merge somehow so we can append new config without having to pass the entire object again?)
            outputs.AddRange(resourceMockOutputs.First());
            
            object physicalResourceName = outputs.GetValueOrDefault("name") ?? $"{GetLogicalResourceName(args.Name)}_physical";
            outputs.Add("name", physicalResourceName);
        }
        
        outputs.AddRange(args.Inputs);
        ImmutableDictionary<string, object> finalOutputs = outputs.ToImmutable();
        
        string resourceName = GetLogicalResourceName(args.Name);
        string resourceId = GetResourceId(args.Id, $"{resourceName}_id");
        
        _inputs.Add(new Input(resourceName, finalOutputs));
        return Task.FromResult<(string?, object)>((resourceId, finalOutputs));
    }

    public Task<object> CallAsync(MockCallArgs args)
    {
        ImmutableDictionary<string, object>.Builder outputs = ImmutableDictionary.CreateBuilder<string, object>();
        
        IEnumerable<ImmutableDictionary<string, object>> callMockOutputs = mockCalls
            .Where(mockCall => mockCall.Type.MatchesCallTypeToken(GetCallToken(args.Token)))
            .Select(mockCall => mockCall.MockOutputs);
        
        // TODO: don't add all, select latest added (or merge somehow so we can append new config without having to pass the entire object again?)
        outputs.AddRange(callMockOutputs.First());
        
        ImmutableDictionary<string, object> finalOutputs = outputs.ToImmutable();
        
        // TODO: what if two calls are made with the same token?
        _inputs.Add(new Input(GetCallToken(args.Token), finalOutputs));
        
        return Task.FromResult<object>(finalOutputs);
    }
    
    private static string GetLogicalResourceName(string? name) =>
        string.IsNullOrWhiteSpace(name) ? throw new ArgumentNullException(nameof(name)) : name;
    
    private static string GetResourceId(string? id, string fallbackId) =>
        string.IsNullOrWhiteSpace(id) ? fallbackId : id;
    
    private static string GetCallToken(string? token) =>
        string.IsNullOrWhiteSpace(token) ? throw new ArgumentNullException(nameof(token)) : token;
}
