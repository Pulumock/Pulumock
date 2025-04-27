using System.Collections.Immutable;
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
            new TestOptions {IsPreview = false},
            async () => await CoreStack.DefineResourcesAsync(StackName));
            
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
            new TestOptions {IsPreview = false},
            async () => await CoreStack.DefineResourcesAsync(StackName));
        
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
            new TestOptions {IsPreview = false},
            async () => await CoreStack.DefineResourcesAsync(StackName));
        
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
            new TestOptions {IsPreview = false},
            async () => await CoreStack.DefineResourcesAsync(StackName));
        
        ResourceGroup resourceGroup = result.Resources
            .OfType<ResourceGroup>()
            .Single(x => x.GetResourceName().Equals("microservice-rg", StringComparison.Ordinal));
        
        ResourceSnapshot resourceSnapshot = mocks.ResourceSnapshots.Single(x => x.LogicalName.Equals("microservice-kv-vault", StringComparison.Ordinal));
        if (!resourceSnapshot.Inputs.TryGetValue("resourceGroupName", out object? value) || value is not string resourceGroupName)
        {
            throw new KeyNotFoundException("Input with key 'resourceGroupName' was not found or is not of type string.");
        }
        
        resourceGroupName.ShouldBe(await OutputUtilities.GetValueAsync(resourceGroup.Name));
    }
    
    [Fact]
    public async Task Resource_Multiple()
    {
        (ImmutableArray<Resource> Resources, IDictionary<string, object?> StackOutputs) result = await Deployment.TestAsync(
            new Mocks.Mocks(), 
            new TestOptions {IsPreview = false},
            async () => await CoreStack.DefineResourcesAsync(StackName));

        IEnumerable<Resource> resources = result.Resources
            .OfType<Resource>();
        
        resources.ShouldAllBe(x => !string.IsNullOrWhiteSpace(x.GetResourceName()));
    }
}
