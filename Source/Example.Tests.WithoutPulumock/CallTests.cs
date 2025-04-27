using System.Collections.Immutable;
using Example.Stacks;
using Example.Tests.Shared.Interfaces;
using Example.Tests.WithoutPulumock.Shared;
using Pulumi;
using Pulumi.AzureNative.Authorization;
using Pulumi.Testing;
using Pulumi.Utilities;
using Shouldly;

namespace Example.Tests.WithoutPulumock;

public class CallTests : TestBase, ICallTests
{
    [Fact]
    public async Task Call_Input()
    {
        var mocks = new Mocks.Mocks();
        _ = await Deployment.TestAsync(
            mocks, 
            new TestOptions {IsPreview = false},
            async () => await CoreStack.DefineResourcesAsync(StackName));
        
        var calls = mocks.CallSnapshots
            .Where(x => x.Token.Equals("azure-native:authorization:getRoleDefinition", StringComparison.Ordinal))
            .ToList();
        
        var roleDefinitionIds = calls
            .Select(call => call.Inputs.TryGetValue("roleDefinitionId", out object? roleDefinitionId) ? roleDefinitionId as string : null)
            .Where(id => id is not null)
            .ToList();
        
        calls.Count.ShouldBe(2);
        roleDefinitionIds.ShouldContain("b24988ac-6180-42a0-ab88-20f7382dd24c");
        roleDefinitionIds.ShouldContain("88fa32db-c830-43a9-88bc-fa482a8401e8");
    }
    
    [Fact]
    public async Task Call_Output()
    {
        var mocks = new Mocks.Mocks();
        _ = await Deployment.TestAsync(
            mocks, 
            new TestOptions {IsPreview = false},
            async () => await CoreStack.DefineResourcesAsync(StackName));
        
        var calls = mocks.CallSnapshots
            .Where(x =>
                x.Token.Equals("azure-native:authorization:getRoleDefinition", StringComparison.Ordinal) 
                && x.Inputs.TryGetValue("roleDefinitionId", out object? roleDefinitionId) 
                && roleDefinitionId is string id 
                && id.Equals("b24988ac-6180-42a0-ab88-20f7382dd24c", StringComparison.Ordinal))
            .ToList();
        
        var getRoleDefinitionIds = calls
            .Select(call => call.Outputs.TryGetValue("id", out object? id) 
                ? id as string 
                : throw new InvalidOperationException("Output 'id' was not found."))
            .ToList();
        
        getRoleDefinitionIds.ShouldAllBe(id => string.Equals(id, "b24988ac-6180-42a0-ab88-20f7382dd24c", StringComparison.Ordinal));
    }
    
    [Fact]
    public async Task Call_ResourceDependency()
    {
        var mocks = new Mocks.Mocks();
        (ImmutableArray<Resource> Resources, IDictionary<string, object?> StackOutputs) result = await Deployment.TestAsync(
            mocks, 
            new TestOptions {IsPreview = false},
            async () => await CoreStack.DefineResourcesAsync(StackName));
        
        RoleAssignment roleAssignment = result.Resources
            .OfType<RoleAssignment>()
            .Single(x => x.GetResourceName().Equals("microservice-ra-kvReader", StringComparison.Ordinal));
        
        string roleDefinitionId = await OutputUtilities.GetValueAsync(roleAssignment.RoleDefinitionId);
        roleDefinitionId.ShouldBe("b24988ac-6180-42a0-ab88-20f7382dd24c");
    }
}
