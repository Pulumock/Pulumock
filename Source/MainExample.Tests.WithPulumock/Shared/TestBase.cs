using Pulumi.AzureNative.Authorization;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.Resources;
using Pulumock.Mocks.Builders;
using Pulumock.Mocks.Enums;
using Pulumock.TestFixtures;

namespace MainExample.Tests.WithPulumock.Shared;

#pragma warning disable CA1515
public class TestBase
#pragma warning restore CA1515
{
    private readonly FixtureBuilder _fixtureBuilder;
    
    protected TestBase() =>
        _fixtureBuilder = new FixtureBuilder()
            .WithMockConfiguration(new MockConfigurationBuilder()
                .WithConfiguration(PulumiConfigurationNamespace.AzureNative, "tenantId", "1f526cdb-1975-4248-ab0f-57813df294cb")
                .WithConfiguration(PulumiConfigurationNamespace.AzureNative, "subscriptionId", "f2f2c6e5-17c2-4dfa-913d-6509deb6becf")
                .WithConfiguration(PulumiConfigurationNamespace.AzureNative, "location", "swedencentral")
                .WithConfiguration(PulumiConfigurationNamespace.Default, "useKeyVaultWithSecretsComponentResource", "true")
                .WithSecretConfiguration(PulumiConfigurationNamespace.Default, "databaseConnectionString", "very-secret-value")
                .Build())
            .WithMockStackReference(new MockStackReferenceBuilder($"hoolit/Identity/{StackName}")
                .WithOutput("microserviceManagedIdentityPrincipalId", "b95a4aa0-167a-4bc2-baf4-d43a776da1bd")
                .Build())
            .WithMockResource(new MockResourceBuilder()
                .WithOutput<ResourceGroup>(x => x.AzureApiVersion, "2021-04-01")
                .Build<ResourceGroup>())
            .WithMockResource(new MockResourceBuilder()
                .WithOutput<GetVaultResult>(x => x.Properties.VaultUri, "https://mocked.vault.azure.net/")
                .Build<Vault>())
            .WithMockCall(new MockCallBuilder()
                .WithOutput<GetRoleDefinitionResult>(x => x.Id, "13a8e88e-f45f-432b-8b45-019997c19f27")
                .Build(typeof(GetRoleDefinition)));
    
    public FixtureBuilder FixtureBuilder => _fixtureBuilder;
    
    public const string StackName = "dev";
}
