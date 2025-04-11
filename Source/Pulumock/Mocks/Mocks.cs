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
    public Task<(string? id, object state)> NewResourceAsync(MockResourceArgs args)
    {
        ImmutableDictionary<string, object>.Builder outputs = ImmutableDictionary.CreateBuilder<string, object>();
        
        if (string.Equals(args.Type, ResourceTypeTokenConstants.StackReference, StringComparison.Ordinal))
        {
            ImmutableDictionary<string, object> mockOutputs = mockResources
                .OfType<MockStackReference>()
                .Single(stackReferences => string.Equals(stackReferences.FullyQualifiedStackName, args.Name, StringComparison.Ordinal))
                .MockOutputs;
            
            outputs.Add("outputs", mockOutputs);
            outputs.Add("secretOutputNames", ImmutableArray<string>.Empty);
        }
        else
        {
            IEnumerable<ImmutableDictionary<string, object>> resourceMockOutputs = mockResources
                .Where(mockResource => mockResource.Type.MatchesResourceTypeToken(args.Type))
                .Select(mockResource => mockResource.MockOutputs);
            
            foreach (ImmutableDictionary<string, object> mockOutputs in resourceMockOutputs)
            {
                outputs.AddRange(mockOutputs);
            }
            
            object physicalResourceName = outputs.GetValueOrDefault("name") ?? $"{args.Name}_name";
            outputs.Add("name", physicalResourceName);
        }
        
        string resourceId = string.IsNullOrWhiteSpace(args.Id) ? $"{args.Name}_id" : args.Id;
        outputs.AddRange(args.Inputs);
        return Task.FromResult<(string?, object)>((resourceId, outputs));
    }

    public Task<object> CallAsync(MockCallArgs args)
    {
        IEnumerable<ImmutableDictionary<string, object>> callMockOutputs = mockCalls
            .Where(mockCall => mockCall.Type.MatchesCallTypeToken(args.Token))
            .Select(mockCall => mockCall.MockOutputs);
        
        return Task.FromResult<object>(callMockOutputs);
    }
}
