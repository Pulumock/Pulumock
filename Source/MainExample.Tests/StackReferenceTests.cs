using System.Collections.Immutable;
using MainExample.Stacks;
using MainExample.Tests.Shared;
using MainExample.Tests.Shared.Interfaces;
using Pulumi;
using Pulumi.AzureNative.Authorization;
using Pulumi.Testing;
using Pulumi.Utilities;
using Shouldly;

namespace MainExample.Tests;

public class StackReferenceTests : TestBase, IStackReferenceTests
{
    [Fact]
    public async Task StackReference_ShouldUseMockedStackReferenceInResource()
    {
        (ImmutableArray<Resource> Resources, IDictionary<string, object?> StackOutputs) result = await Deployment.TestAsync(
            new Mocks.Mocks(), 
            new TestOptions {IsPreview = false},
            async () => await CoreStack.DefineResourcesAsync(StackName));

        RoleAssignment roleAssignment = result.Resources
            .OfType<RoleAssignment>()
            .Single(x => x.GetResourceName().Equals("microservice-ra-kvReader", StringComparison.Ordinal));
        
        string principalId = await OutputUtilities.GetValueAsync(roleAssignment.PrincipalId);
        principalId.ShouldBe("b95a4aa0-167a-4bc2-baf4-d43a776da1bd");
    }
}
