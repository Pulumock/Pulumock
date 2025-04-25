using System.Collections.Immutable;
using MainExample.Stacks;
using MainExample.Tests.Shared;
using Pulumi;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.Resources;
using Pulumi.Testing;
using Pulumi.Utilities;
using Shouldly;
using Deployment = Pulumi.Deployment;
using Resource = Pulumi.Resource;

namespace MainExample.Tests;

public class ResourceTests : TestBase
{
    /// <summary>
    /// Verifies that an Input-only property (<see cref="Pulumi.AzureNative.Resources.ResourceGroupArgs.ResourceGroupName"/>) is correctly passed to the resource.
    /// Since Input-only values are not returned from <see cref="CustomResource"/>, they must be tracked in the <see cref="IMocks"/> implementation.
    /// </summary>
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
    
    /// <summary>
    /// Validates an Output-only property (<see cref="Pulumi.AzureNative.Resources.ResourceGroup.AzureApiVersion"/>).
    /// Since it has no input, its value must be mocked and asserted from the resource's Output.
    /// </summary>
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
    
    /// <summary>
    /// <para>
    /// Confirms that a property defined as both an Input and an Output 
    /// (<see cref="ResourceGroupArgs.Location"/> and <see cref="ResourceGroup.Location"/>) 
    /// is preserved consistently from the input to the resulting resource output.
    /// </para>
    /// 
    /// <para>
    /// If a property is defined as both an Input and an Output in the provider schema, 
    /// and the provider does not compute or override its value, 
    /// the Input value is implicitly returned as the Output.
    /// This allows the value to be asserted from either the original input or the resulting resource output
    /// without requiring explicit mocking.
    /// </para>
    /// </summary>
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
    
    /// <summary>
    /// Tests a dependency where one resource's Input (<see cref="VaultArgs.ResourceGroupName"/>) 
    /// depends on another resource's Output (<see cref="ResourceGroup.Name"/>), ensuring correct value propagation.
    /// </summary>
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
    
    /// <summary>
    /// Asserting on multiple resources
    /// </summary>
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
