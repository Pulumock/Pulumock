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
    
    [Fact]
    public async Task Call_Output()
    {
        const string getRoleDefinitionId = "b24988ac-6180-42a0-ab88-20f7382dd24c";
        Fixture fixture = await TestBase.GetBaseFixtureBuilder()
            .WithMockCall(new MockCallBuilder()
                .WithOutput<GetRoleDefinitionResult>(x => x.Id, getRoleDefinitionId)
                .Build(typeof(GetRoleDefinition)))
            .BuildAsync(async () => await CoreStack.DefineResourcesAsync());
        
        ImmutableList<CallSnapshot> getRoleDefinitionCallsWithId = fixture.CallSnapshots.GetManyByValue<GetRoleDefinitionInvokeArgs, string>(
            typeof(GetRoleDefinition),
            x => x.RoleDefinitionId,
            getRoleDefinitionId);

        ImmutableList<string> getRoleDefinitionIds = getRoleDefinitionCallsWithId
            .RequireManyOutputValues<GetRoleDefinitionResult, string>(x => x.Id);
        
        getRoleDefinitionIds.ShouldAllBe(id => string.Equals(id, getRoleDefinitionId, StringComparison.Ordinal));
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
