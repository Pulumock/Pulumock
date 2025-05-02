using System.Collections.Immutable;
using System.Text.Json;
using Example.Stacks;
using Example.Tests.Shared.Interfaces;
using Example.Tests.WithoutPulumock.Shared;
using Pulumi.AzureNative.Resources;
using Pulumi.Testing;
using Pulumi.Utilities;
using Shouldly;
using Deployment = Pulumi.Deployment;
using Resource = Pulumi.Resource;

namespace Example.Tests.WithoutPulumock;

public class ResourceTests : TestBase, IResourceTests
{
    [Fact]
    public async Task Resource_InputOnly()
    {
        var mocks = new Mocks.Mocks();
        _ = await Deployment.TestAsync(
            mocks, 
            new TestOptions {IsPreview = false, StackName = DevStackName},
            async () => await CoreStack.DefineResourcesAsync());
            
        ResourceSnapshot resourceSnapshot = mocks.ResourceSnapshots.Single(x => x.LogicalName.Equals("microservice-rg", StringComparison.Ordinal));
        if (!resourceSnapshot.Inputs.TryGetValue("resourceGroupName", out object? value) || value is not string resourceGroupName)
        {
            throw new KeyNotFoundException("Input with key 'resourceGroupName' was not found or is not of type string.");
        }
        
        resourceGroupName.ShouldBe("microservice-rg");
    }
    
    [Fact]
    public async Task Resource_OutputOnly()
    {
        (ImmutableArray<Resource> Resources, IDictionary<string, object?> StackOutputs) result = await Deployment.TestAsync(
            new Mocks.Mocks(), 
            new TestOptions {IsPreview = false, StackName = DevStackName},
            async () => await CoreStack.DefineResourcesAsync());
        
        ResourceGroup resourceGroup = result.Resources
            .OfType<ResourceGroup>()
            .Single(x => x.GetResourceName().Equals("microservice-rg", StringComparison.Ordinal));
        
        string azureApiVersion = await OutputUtilities.GetValueAsync(resourceGroup.AzureApiVersion);
        azureApiVersion.ShouldBe("2021-04-01");
    }
    
    [Fact]
    public async Task Resource_InputOutput()
    {
        var mocks = new Mocks.Mocks();
        (ImmutableArray<Resource> Resources, IDictionary<string, object?> StackOutputs) result = await Deployment.TestAsync(
            mocks, 
            new TestOptions {IsPreview = false, StackName = DevStackName},
            async () => await CoreStack.DefineResourcesAsync());
        
        ResourceGroup resourceGroup = result.Resources
            .OfType<ResourceGroup>()
            .Single(x => x.GetResourceName().Equals("microservice-rg", StringComparison.Ordinal));
        
        ResourceSnapshot resourceSnapshot = mocks.ResourceSnapshots.Single(x => x.LogicalName.Equals("microservice-rg", StringComparison.Ordinal));
        if (!resourceSnapshot.Inputs.TryGetValue("location", out object? value) || value is not string locationFromInput)
        {
            throw new KeyNotFoundException("Input with key 'location' was not found or is not of type string.");
        }
        
        string locationFromOutput = await OutputUtilities.GetValueAsync(resourceGroup.Location);
        
        locationFromInput.ShouldBe("swedencentral");
        locationFromOutput.ShouldBe("swedencentral");
    }
    
    [Fact]
    public async Task Resource_Dependency()
    {
        var mocks = new Mocks.Mocks();
        (ImmutableArray<Resource> Resources, IDictionary<string, object?> StackOutputs) result = await Deployment.TestAsync(
            mocks, 
            new TestOptions {IsPreview = false, StackName = DevStackName},
            async () => await CoreStack.DefineResourcesAsync());
        
        ResourceGroup resourceGroup = result.Resources
            .OfType<ResourceGroup>()
            .Single(x => x.GetResourceName().Equals("microservice-rg", StringComparison.Ordinal));
        
        ResourceSnapshot resourceSnapshot = mocks.ResourceSnapshots.Single(x => x.LogicalName.Equals("microservice-kvws-kv", StringComparison.Ordinal));
        if (!resourceSnapshot.Inputs.TryGetValue("resourceGroupName", out object? value) || value is not string resourceGroupName)
        {
            throw new KeyNotFoundException("Input with key 'resourceGroupName' was not found or is not of type string.");
        }
        
        resourceGroupName.ShouldBe(await OutputUtilities.GetValueAsync(resourceGroup.Name));
    }
    
    [Fact]
    public async Task Resource_Multiple()
    {
        const string location = "norway";
        string? existingConfigJson = Environment.GetEnvironmentVariable("PULUMI_CONFIG");
        Dictionary<string, object> config = existingConfigJson != null
            ? JsonSerializer.Deserialize<Dictionary<string, object>>(existingConfigJson)!
            : new Dictionary<string, object>();
        
        config["azure-native:location"] = location;
        Environment.SetEnvironmentVariable("PULUMI_CONFIG", JsonSerializer.Serialize(config));
        
        (ImmutableArray<Resource> Resources, IDictionary<string, object?> StackOutputs) result = await Deployment.TestAsync(
            new Mocks.Mocks(), 
            new TestOptions {IsPreview = false, StackName = DevStackName},
            async () => await CoreStack.DefineResourcesAsync());

        var resourceGroups = result.Resources
            .OfType<ResourceGroup>()
            .ToImmutableArray();
        
        string?[] locations = await Task.WhenAll(
            resourceGroups.Select(x => OutputUtilities.GetValueAsync(x.Location))
        );
        
        resourceGroups.ShouldAllBe(x => !string.Equals(x.GetResourceName(), "forbidden-resource-name", StringComparison.Ordinal));
        locations.ShouldAllBe(x => !string.IsNullOrWhiteSpace(x) && x.Equals(location, StringComparison.Ordinal));
    }
}
