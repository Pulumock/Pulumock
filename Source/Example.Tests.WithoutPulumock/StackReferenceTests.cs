using System.Collections.Immutable;
using Example.Stacks;
using Example.Tests.Shared.Interfaces;
using Example.Tests.WithoutPulumock.Mocks;
using Example.Tests.WithoutPulumock.Shared;
using Pulumi;
using Pulumi.AzureNative.Authorization;
using Pulumi.Testing;
using Pulumi.Utilities;
using Shouldly;

namespace Example.Tests.WithoutPulumock;

public class StackReferenceTests : TestBase, IStackReferenceTests
{
    [Theory]
    [InlineData(DevStackName)]
    [InlineData(ProdStackName)]
    public async Task ShouldBeTestable_DynamicFullyQualifiedStackName(string stackName)
    {
        const string stackReferenceOutputValue = "b95a4aa0-167a-4bc2-baf4-d43a776da1bd";
        
        (ImmutableArray<Resource> Resources, IDictionary<string, object?> StackOutputs) result = await Deployment.TestAsync(
            new MocksStackReferenceTests(true, stackReferenceOutputValue), 
            new TestOptions {IsPreview = false, StackName = stackName},
            async () => await CoreStack.DefineResourcesAsync());

        RoleAssignment roleAssignment = result.Resources
            .OfType<RoleAssignment>()
            .Single(x => x.GetResourceName().Equals("microservice-ra-kvReader", StringComparison.Ordinal));
        
        string principalId = await OutputUtilities.GetValueAsync(roleAssignment.PrincipalId);
        principalId.ShouldBe(stackReferenceOutputValue);
    }
    
    [Theory]
    [InlineData("b95a4aa0-167a-4bc2-baf4-d43a776da1bd")]
    [InlineData("f08c1742-0d79-47cb-9c14-bf85cf4f5739")]
    public async Task ShouldBeTestable_DynamicValidOutputValues(string stackReferenceOutputValue) 
    {
        (ImmutableArray<Resource> Resources, IDictionary<string, object?> StackOutputs) result = await Deployment.TestAsync(
            new MocksStackReferenceTests(true, stackReferenceOutputValue), 
            new TestOptions {IsPreview = false, StackName = DevStackName},
            async () => await CoreStack.DefineResourcesAsync());

        RoleAssignment roleAssignment = result.Resources
            .OfType<RoleAssignment>()
            .Single(x => x.GetResourceName().Equals("microservice-ra-kvReader", StringComparison.Ordinal));
        
        string principalId = await OutputUtilities.GetValueAsync(roleAssignment.PrincipalId);
        principalId.ShouldBe(stackReferenceOutputValue);
    }
    
    [Fact]
    public async Task ShouldBeTestable_MissingOutputValue() =>
        await Should.ThrowAsync<RunException>(async () =>
        {
            _ = await Deployment.TestAsync(
                new MocksStackReferenceTests(false), 
                new TestOptions {IsPreview = false, StackName = DevStackName},
                async () => await CoreStack.DefineResourcesAsync());
        });
    
    [Fact]
    public async Task ShouldBeTestable_InvalidOutputValueType() =>
        await Should.ThrowAsync<RunException>(async () =>
        {
            _ = await Deployment.TestAsync(
                new MocksStackReferenceTests(true, true), 
                new TestOptions {IsPreview = false, StackName = DevStackName},
                async () => await CoreStack.DefineResourcesAsync());
        });
}
