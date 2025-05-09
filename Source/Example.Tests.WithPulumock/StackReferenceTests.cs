using Example.Stacks;
using Example.Tests.Shared.Interfaces;
using Example.Tests.WithPulumock.Shared;
using Pulumi;
using Pulumi.AzureNative.Authorization;
using Pulumi.Testing;
using Pulumock.Extensions;
using Pulumock.Mocks.Builders;
using Pulumock.TestFixtures;
using Shouldly;

namespace Example.Tests.WithPulumock;

public class StackReferenceTests : IStackReferenceTests
{
    [Theory]
    [InlineData(TestBase.DevStackName)]
    [InlineData(TestBase.ProdStackName)]
    public async Task ShouldBeTestable_DynamicFullyQualifiedStackName(string stackName)
    {
        const string stackReferenceOutputValue = "b95a4aa0-167a-4bc2-baf4-d43a776da1bd";
        
        Fixture fixture = await TestBase.GetBaseFixtureBuilder()
            .WithMockStackReference(new MockStackReferenceBuilder($"hoolit/StackReference/{stackName}")
                .WithOutput("microserviceManagedIdentityPrincipalId", stackReferenceOutputValue)
                .Build())
            .BuildAsync(async () => await CoreStack.DefineResourcesAsync(),
                new TestOptions { IsPreview = false, StackName = stackName });

        RoleAssignment roleAssignment = fixture.Resources.Require<RoleAssignment>("microservice-ra-kvReader");
        string principalId = await roleAssignment.PrincipalId.GetValueAsync();
        
        principalId.ShouldBe(stackReferenceOutputValue);
    }
    
    [Theory]
    [InlineData("b95a4aa0-167a-4bc2-baf4-d43a776da1bd")]
    [InlineData("f08c1742-0d79-47cb-9c14-bf85cf4f5739")]
    public async Task ShouldBeTestable_DynamicValidOutputValues(string stackReferenceOutputValue)
    {
        Fixture fixture = await TestBase.GetBaseFixtureBuilder()
            .WithMockStackReference(new MockStackReferenceBuilder($"hoolit/StackReference/{TestBase.DevStackName}")
                .WithOutput("microserviceManagedIdentityPrincipalId", stackReferenceOutputValue)
                .Build())
            .BuildAsync(async () => await CoreStack.DefineResourcesAsync(),
                new TestOptions { IsPreview = false, StackName = TestBase.DevStackName });

        RoleAssignment roleAssignment = fixture.Resources.Require<RoleAssignment>("microservice-ra-kvReader");
        string principalId = await roleAssignment.PrincipalId.GetValueAsync();
        
        principalId.ShouldBe(stackReferenceOutputValue);
    }
    
    [Fact]
    public async Task ShouldBeTestable_MissingOutputValue() =>
        await Should.ThrowAsync<RunException>(async () =>
        {
            _ = await TestBase.GetBaseFixtureBuilder()
                .WithoutMockStackReference($"hoolit/StackReference/{TestBase.DevStackName}")
                .BuildAsync(async () => await CoreStack.DefineResourcesAsync(),
                    new TestOptions { IsPreview = false, StackName = TestBase.DevStackName });
        });
    
    [Fact]
    public async Task ShouldBeTestable_InvalidOutputValueType() =>
        await Should.ThrowAsync<RunException>(async () =>
        {
            const bool invalidStackReferenceOutputValue = true;
            
            _ = await TestBase.GetBaseFixtureBuilder()
                .WithMockStackReference(new MockStackReferenceBuilder($"hoolit/StackReference/{TestBase.DevStackName}")
                    .WithOutput("microserviceManagedIdentityPrincipalId", invalidStackReferenceOutputValue)
                    .Build())
                .BuildAsync(async () => await CoreStack.DefineResourcesAsync(),
                    new TestOptions { IsPreview = false, StackName = TestBase.DevStackName });
        });
}
