using System.Collections.Immutable;
using Example.ComponentResources;
using Example.Tests.Shared.Interfaces;
using Example.Tests.WithoutPulumock.Shared;
using Pulumi;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.KeyVault.Outputs;
using Pulumi.Testing;
using Pulumi.Utilities;
using Shouldly;

namespace Example.Tests.WithoutPulumock;

public class ComponentResourceTests : TestBase, IComponentResourceTests
{
    [Fact]
    public async Task ComponentResource()
    {
        var mocks = new Mocks.Mocks();
        (ImmutableArray<Resource> Resources, IDictionary<string, object?> Outputs) result = await Deployment.TestAsync(
            mocks, 
            new TestOptions {IsPreview = false},
            () =>
            {
                var componentResource = new KeyVaultWithSecretsComponentResource("microservice-kvws", new()
                {
                    VaultName = "microservice-kv",
                    ResourceGroupName = "microservice-rg",
                    TenantId = "1f526cdb-1975-4248-ab0f-57813df294cb",
                    Secrets = new()
                    {
                        {"Database--ConnectionString", "very-secret-value"}
                    }
                });
                
                return new Dictionary<string, object?>
                {
                    { "keyVault", componentResource.KeyVault }
                };
            });

        KeyVaultWithSecretsComponentResource componentResource = result.Resources
            .OfType<KeyVaultWithSecretsComponentResource>()
            .Single();
        
        Vault keyVault = result.Resources
            .OfType<Vault>()
            .Single();

        Secret secret = result.Resources
            .OfType<Secret>()
            .Single();
        
        SecretPropertiesResponse secretProperties = await OutputUtilities.GetValueAsync(secret.Properties);

        componentResource.GetResourceName().ShouldBe("microservice-kvws");
        keyVault.GetResourceName().ShouldBe("microservice-kvws-kv");
        secret.GetResourceName().ShouldBe("microservice-kvws-secret-Database--ConnectionString");
        secretProperties.Value.ShouldBe("very-secret-value");
    }
    
    [Fact]
    public async Task ComponentResource_MissingNonRequiredResourceArg()
    {
        var mocks = new Mocks.Mocks();
        (ImmutableArray<Resource> Resources, IDictionary<string, object?> Outputs) result = await Deployment.TestAsync(
            mocks, 
            new TestOptions {IsPreview = false},
            () =>
            {
                var componentResource = new KeyVaultWithSecretsComponentResource("microservice-kv", new()
                {
                    VaultName = "microservice-kv-vault",
                    ResourceGroupName = "microservice-rg",
                    TenantId = "1f526cdb-1975-4248-ab0f-57813df294cb"
                });
                
                return new Dictionary<string, object?>
                {
                    { "keyVault", componentResource.KeyVault }
                };
            });

        Secret? secret = result.Resources
            .OfType<Secret>()
            .SingleOrDefault();
        
        secret.ShouldBeNull();
    }
    
    [Fact]
    public async Task ComponentResource_Parent()
    {
        var mocks = new Mocks.Mocks();
        (ImmutableArray<Resource> Resources, IDictionary<string, object?> Outputs) result = await Deployment.TestAsync(
            mocks, 
            new TestOptions {IsPreview = false},
            () =>
            {
                var componentResource = new KeyVaultWithSecretsComponentResource("microservice-kv", new()
                {
                    VaultName = "microservice-kv-vault",
                    ResourceGroupName = "microservice-rg",
                    TenantId = "1f526cdb-1975-4248-ab0f-57813df294cb",
                    Secrets = new()
                    {
                        {"Database--ConnectionString", "very-secret-value"}
                    }
                });
                
                return new Dictionary<string, object?>
                {
                    { "keyVault", componentResource.KeyVault }
                };
            });

        KeyVaultWithSecretsComponentResource componentResource = result.Resources
            .OfType<KeyVaultWithSecretsComponentResource>()
            .Single();
        
        Vault keyVault = result.Resources
            .OfType<Vault>()
            .Single();
        
        Secret secret = result.Resources
            .OfType<Secret>()
            .Single();
        
        TestHelpers.IsChildOf(keyVault, componentResource).ShouldBeTrue();
        TestHelpers.IsChildOf(secret, componentResource).ShouldBeTrue();
    }
    
    [Fact]
    public async Task ComponentResource_Outputs()
    {
        var mocks = new Mocks.Mocks();
        (ImmutableArray<Resource> Resources, IDictionary<string, object?> Outputs) result = await Deployment.TestAsync(
            mocks, 
            new TestOptions {IsPreview = false},
            () =>
            {
                var componentResource = new KeyVaultWithSecretsComponentResource("microservice-kvws", new()
                {
                    VaultName = "microservice-kv",
                    ResourceGroupName = "microservice-rg",
                    TenantId = "1f526cdb-1975-4248-ab0f-57813df294cb"
                });
                
                return new Dictionary<string, object?>
                {
                    { "keyVault", componentResource.KeyVault }
                };
            });

        if (!result.Outputs.TryGetValue("keyVault", out object? keyVaultObj) || keyVaultObj is not Vault keyVault)
        {
            throw new KeyNotFoundException("Output with key 'keyVault' was not found or is not of type Vault.");
        }
        
        keyVault.GetResourceName().ShouldBe("microservice-kvws-kv");
    }
}
