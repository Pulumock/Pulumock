using System.Collections.Immutable;
using Example.Stacks;
using Example.Tests.Shared.Interfaces;
using Example.Tests.WithPulumock.Shared;
using Pulumi.AzureNative.Authorization;
using Pulumock.Extensions;
using Pulumock.Mocks.Builders;
using Pulumock.Mocks.Models;
using Pulumock.TestFixtures;
using Shouldly;

namespace Example.Tests.WithPulumock;

public class CallTests : ICallTests
{
    [Fact]
    public async Task Call_Input()
    {
        Fixture fixture = await TestBase.GetBaseFixtureBuilder()
            .BuildAsync(async () => await CoreStack.DefineResourcesAsync());

        ImmutableList<CallSnapshot> getRoleDefinitionCalls = fixture.CallSnapshots.GetMany(typeof(GetRoleDefinition));
        ImmutableList<string> roleDefinitionIds = getRoleDefinitionCalls.RequireManyInputValues<GetRoleDefinitionInvokeArgs, string>(x => 
            x.RoleDefinitionId);
        
        getRoleDefinitionCalls.Count.ShouldBe(2);
        roleDefinitionIds.ShouldContain("b24988ac-6180-42a0-ab88-20f7382dd24c");
        roleDefinitionIds.ShouldContain("7f951dda-4ed3-4680-a7ca-43fe172d538d");
    }

    // TODO: complete
    [Fact]
    public async Task Call_Output()
    {
        Fixture fixture = await TestBase.GetBaseFixtureBuilder()
            .BuildAsync(async () => await CoreStack.DefineResourcesAsync());
        
        ImmutableList<CallSnapshot> getRoleDefinitionCalls = fixture.CallSnapshots.GetMany(typeof(GetRoleDefinition));
        
        // var calls = mocks.CallSnapshots
        //     .Where(x =>
        //         x.Token.Equals("azure-native:authorization:getRoleDefinition", StringComparison.Ordinal) 
        //         && x.Inputs.TryGetValue("roleDefinitionId", out object? roleDefinitionId) 
        //         && roleDefinitionId is string id 
        //         && id.Equals("b24988ac-6180-42a0-ab88-20f7382dd24c", StringComparison.Ordinal))
        //     .ToList();
        //
        // var getRoleDefinitionIds = calls
        //     .Select(call => call.Outputs.TryGetValue("id", out object? id) 
        //         ? id as string 
        //         : throw new InvalidOperationException("Output 'id' was not found."))
        //     .ToList();
        //
        // getRoleDefinitionIds.ShouldAllBe(id => string.Equals(id, "b24988ac-6180-42a0-ab88-20f7382dd24c", StringComparison.Ordinal));
    }

    [Fact]
    public async Task Call_ResourceDependency()
    {
        const string getRoleDefinitionId = "b24988ac-6180-42a0-ab88-20f7382dd24c";
        Fixture fixture = await TestBase.GetBaseFixtureBuilder()
            .WithMockCall(new MockCallBuilder()
                .WithOutput<GetRoleDefinitionResult>(x => x.Id, getRoleDefinitionId)
                .Build(typeof(GetRoleDefinition)))
            .BuildAsync(async () => await CoreStack.DefineResourcesAsync());
        
        RoleAssignment roleAssignment = fixture.StackResources.Require<RoleAssignment>("microservice-ra-kvReader");

        string roleDefinitionId = await roleAssignment.RoleDefinitionId.GetValueAsync();
        roleDefinitionId.ShouldBe(getRoleDefinitionId);
    }
}
