using System.Collections.Immutable;
using Example.Tests.WithoutPulumock.Shared;
using Pulumi.Testing;

namespace Example.Tests.WithoutPulumock.Mocks;

internal sealed class MocksStackReferenceTests(bool mock, object? value = null) : MocksBase
{
    public override Task<(string? id, object state)> NewResourceAsync(MockResourceArgs args)
    {
        if (!string.Equals(args.Type, "pulumi:pulumi:StackReference", StringComparison.Ordinal) ||
            !string.Equals($"hoolit/StackReference/{DevStackName}", args.Name, StringComparison.Ordinal) &&
             !string.Equals($"hoolit/StackReference/{ProdStackName}", args.Name, StringComparison.Ordinal))
        {
            return base.NewResourceAsync(args);
        }

        ImmutableDictionary<string, object>.Builder outputs = ImmutableDictionary.CreateBuilder<string, object>();
        ImmutableDictionary<string, object?>.Builder mockOutputs = ImmutableDictionary.CreateBuilder<string, object?>();
        if (mock)
        {
            mockOutputs.Add("microserviceManagedIdentityPrincipalId", value);
        }
        
        outputs.AddRange(args.Inputs);
        outputs.Add("outputs", mock ? mockOutputs.ToImmutable() : ImmutableDictionary<string, object?>.Empty);
        outputs.Add("secretOutputNames", ImmutableArray<string>.Empty);

        string resourceName = GetLogicalResourceName(args.Name);
        string resourceId = GetResourceId(args.Id, $"{resourceName}_id");

        ProtectedEnrichedResources.Add(new EnrichedResource(resourceName, args.Inputs));
        
        return Task.FromResult<(string? id, object state)>((resourceId, outputs.ToImmutable()));
    }
}
