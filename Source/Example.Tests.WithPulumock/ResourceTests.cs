using System.Collections.Immutable;
using Example.Stacks;
using Example.Tests.Shared.Interfaces;
using Example.Tests.WithPulumock.Shared;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.Resources;
using Pulumock.Extensions;
using Pulumock.Mocks.Builders;
using Pulumock.Mocks.Enums;
using Pulumock.Mocks.Models;
using Pulumock.TestFixtures;
using Shouldly;

namespace Example.Tests.WithPulumock;

public class ResourceTests : IResourceTests
{
    [Fact]
    public async Task Resource_InputOnly()
    {
        Fixture fixture = await TestBase.GetBaseFixtureBuilder()
            .BuildAsync(async () => await CoreStack.DefineResourcesAsync());
        
        ResourceSnapshot resourceGroup = fixture.ResourceSnapshots.Require("microservice-rg");
        string resourceGroupName = resourceGroup.RequireInputValue<ResourceGroupArgs, string>(x => x.ResourceGroupName);
        
        resourceGroupName.ShouldBe("microservice-rg");
    }

    [Fact]
    public async Task Resource_OutputOnly()
    {
        const string expectedAzureApiVersion = "2021-04-01";
        Fixture fixture = await TestBase.GetBaseFixtureBuilder()
            .WithMockResource(new MockResourceBuilder<ResourceGroup>()
                .WithOutput(x => x.AzureApiVersion, expectedAzureApiVersion)
                .Build())
            .BuildAsync(async () => await CoreStack.DefineResourcesAsync());
        
        ResourceGroup resourceGroup = fixture.StackResources.Require<ResourceGroup>("microservice-rg");
        string azureApiVersion = await resourceGroup.AzureApiVersion.GetValueAsync();
        
        azureApiVersion.ShouldBe(expectedAzureApiVersion);
    }

    [Fact]
    public async Task Resource_InputOutput()
    {
        const string location = "swedencentral";
        Fixture fixture = await TestBase.GetBaseFixtureBuilder()
            .WithMockStackConfiguration(PulumiConfigurationNamespace.AzureNative, "location", location)
            .BuildAsync(async () => await CoreStack.DefineResourcesAsync());
        
        ResourceGroup resourceGroup = fixture.StackResources.Require<ResourceGroup>("microservice-rg");
        ResourceSnapshot resourceGroupSnapshot = fixture.ResourceSnapshots.Require("microservice-rg");

        string locationFromOutput = await resourceGroup.Location.GetValueAsync();
        string locationFromInput = resourceGroupSnapshot.RequireInputValue<ResourceGroupArgs, string>(x => x.Location);
        
        locationFromOutput.ShouldBe(location);
        locationFromInput.ShouldBe(location);
    }

    [Fact]
    public async Task Resource_Dependency()
    {
        Fixture fixture = await TestBase.GetBaseFixtureBuilder()
            .BuildAsync(async () => await CoreStack.DefineResourcesAsync());
        
        ResourceGroup resourceGroup = fixture.StackResources.Require<ResourceGroup>("microservice-rg");
        string resourceGroupName = await resourceGroup.Name.GetValueAsync();
        
        ResourceSnapshot keyVault = fixture.ResourceSnapshots.Require("microservice-kvws-kv");
        string keyVaultResourceGroupName = keyVault.RequireInputValue<VaultArgs, string>(x => x.ResourceGroupName);

        keyVaultResourceGroupName.ShouldBe(resourceGroupName);
    }

    [Fact]
    public async Task Resource_Multiple()
    {
        const string location = "norway";
        Fixture fixture = await TestBase.GetBaseFixtureBuilder()
            .WithMockStackConfiguration(PulumiConfigurationNamespace.AzureNative, "location", location)
            .BuildAsync(async () => await CoreStack.DefineResourcesAsync());

        ImmutableArray<ResourceGroup> resourceGroups = fixture.StackResources.GetMany<ResourceGroup>();
        string[] locations = await resourceGroups.GetManyValuesAsync(x => x.Location);
        
        resourceGroups.ShouldAllBe(x => !string.Equals(x.GetResourceName(), "forbidden-resource-name", StringComparison.Ordinal));
        locations.ShouldAllBe(x => !string.IsNullOrWhiteSpace(x) && x.Equals(location, StringComparison.Ordinal));
    }
}
