using System.Collections.Immutable;
using Example.Tests.WithoutPulumock.Shared;
using Pulumi.Testing;

namespace Example.Tests.WithoutPulumock.Mocks;

internal sealed class MocksStackOutputTests(string mockedVaultUri) : MocksBase
{
    public override Task<(string? id, object state)> NewResourceAsync(MockResourceArgs args)
    {
        if (!string.Equals(args.Type, "azure-native:keyvault:Vault", StringComparison.Ordinal))
        {
            return base.NewResourceAsync(args);
        }

        ImmutableDictionary<string, object>.Builder outputs = ImmutableDictionary.CreateBuilder<string, object>();
        ImmutableDictionary<string, object?>.Builder mockOutputs = ImmutableDictionary.CreateBuilder<string, object?>();
        
        outputs.AddRange(args.Inputs);
        
        Dictionary<string, object> properties = outputs.TryGetValue("properties", out object? value) && value is Dictionary<string, object> existing
            ? existing
            : new Dictionary<string, object>();

        properties["vaultUri"] = mockedVaultUri;
        outputs["properties"] = properties;

        string resourceName = GetLogicalResourceName(args.Name);
        string resourceId = GetResourceId(args.Id, $"{resourceName}_id");

        ProtectedResourceSnapshots.Add(new ResourceSnapshot(resourceName, args.Inputs));
        
        return Task.FromResult<(string? id, object state)>((resourceId, outputs.ToImmutable()));
    }
}
