using Example.ComponentResources;
using Example.Tests.Shared.Interfaces;
using Example.Tests.WithPulumock.Shared;
using Pulumi;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.KeyVault.Outputs;
using Pulumock.Extensions;
using Pulumock.TestFixtures;
using Shouldly;

namespace Example.Tests.WithPulumock;

public class ComponentResourceTests : IComponentResourceTests
{
    [Fact]
    public async Task ComponentResource()
    {
        Fixture fixture = await TestBase.GetBaseFixtureBuilder()
            .Build(() =>
            {
                var componentResource = new KeyVaultWithSecretsComponentResource("microservice-kvws", new()
                {
                    VaultName = "microservice-kv",
                    ResourceGroupName = "microservice-rg",
                    TenantId = "1f526cdb-1975-4248-ab0f-57813df294cb",
                    Secrets = new InputMap<string>
                    {
                        {"Database--ConnectionString", "very-secret-value"}
                    }
                });
                
                return new Dictionary<string, object?>
                {
                    { "keyVault", componentResource.KeyVault }
                };
            });

        KeyVaultWithSecretsComponentResource componentResource =
            fixture.Resources.Require<KeyVaultWithSecretsComponentResource>();
        Vault keyVault = fixture.Resources.Require<Vault>();
        Secret secret = fixture.Resources.Require<Secret>();

        SecretPropertiesResponse secretProperties = await secret.Properties.GetValueAsync();
        
        componentResource.GetResourceName().ShouldBe("microservice-kvws");
        keyVault.GetResourceName().ShouldBe("microservice-kvws-kv");
        secret.GetResourceName().ShouldBe("microservice-kvws-secret-Database--ConnectionString");
        secretProperties.Value.ShouldBe("very-secret-value");
    }

    [Fact]
    public async Task ComponentResource_MissingNonRequiredResourceArg()
    {
        Fixture fixture = await TestBase.GetBaseFixtureBuilder()
            .Build(() =>
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

        Secret? secret = fixture.Resources.Get<Secret>();
        
        secret.ShouldBeNull();
    }

    [Fact]
    public async Task ComponentResource_Parent()
    {
        Fixture fixture = await TestBase.GetBaseFixtureBuilder()
            .Build(() =>
            {
                var componentResource = new KeyVaultWithSecretsComponentResource("microservice-kvws", new()
                {
                    VaultName = "microservice-kv",
                    ResourceGroupName = "microservice-rg",
                    TenantId = "1f526cdb-1975-4248-ab0f-57813df294cb",
                    Secrets = new InputMap<string>
                    {
                        {"Database--ConnectionString", "very-secret-value"}
                    }
                });
                
                return new Dictionary<string, object?>
                {
                    { "keyVault", componentResource.KeyVault }
                };
            });

        KeyVaultWithSecretsComponentResource componentResource =
            fixture.Resources.Require<KeyVaultWithSecretsComponentResource>();
        Vault keyVault = fixture.Resources.Require<Vault>();
        Secret secret = fixture.Resources.Require<Secret>();
        
        keyVault.IsChildOf(componentResource).ShouldBeTrue();
        secret.IsChildOf(componentResource).ShouldBeTrue();
        componentResource.HasChildren([keyVault, secret]).ShouldBeTrue();
    }

    [Fact]
    public async Task ComponentResource_Outputs()
    {
        Fixture fixture = await TestBase.GetBaseFixtureBuilder()
            .Build(() =>
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

        Vault keyVault = await fixture.RegisteredOutputs.RequireValueAsync<Vault>("keyVault");
        
        keyVault.GetResourceName().ShouldBe("microservice-kvws-kv");
    }
}
