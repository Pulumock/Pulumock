using Example.Stacks;
using Example.Tests.Shared.Interfaces;
using Example.Tests.WithPulumock.Shared;
using Pulumi.AzureNative.Authorization;
using Pulumi.Testing;
using Pulumock.Extensions;
using Pulumock.Mocks.Builders;
using Pulumock.TestFixtures;
using Shouldly;

namespace Example.Tests.WithPulumock;

public class StackReferenceTests : IStackReferenceTests
{
    [Fact]
    public async Task StackReference_ShouldUseMockedStackReferenceInResource()
    {
        const string stackReferenceValue = "b95a4aa0-167a-4bc2-baf4-d43a776da1bd";
        
        Fixture fixture = await TestBase.GetBaseFixtureBuilder()
            .WithMockStackReference(new MockStackReferenceBuilder($"hoolit/StackReference/{TestBase.DevStackName}")
                .WithOutput("microserviceManagedIdentityPrincipalId", stackReferenceValue)
                .Build())
            .BuildAsync(async () => await CoreStack.DefineResourcesAsync(),
                new TestOptions { IsPreview = false, StackName = TestBase.DevStackName });

        RoleAssignment roleAssignment = fixture.StackResources.Require<RoleAssignment>("microservice-ra-kvReader");
        string principalId = await roleAssignment.PrincipalId.GetValueAsync();
        
        principalId.ShouldBe(stackReferenceValue);
    }
}
