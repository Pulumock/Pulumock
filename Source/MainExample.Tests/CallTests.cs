using System.Collections.Immutable;
using MainExample.Stacks;
using MainExample.Tests.Shared;
using Pulumi;
using Pulumi.AzureNative.Authorization;
using Pulumi.Testing;
using Pulumi.Utilities;
using Shouldly;

namespace MainExample.Tests;

public class CallTests : TestBase
{
    /// <summary>
    /// <para>
    /// Validates that provider function calls, specifically 
    /// <c>azure-native:authorization:getRoleDefinition</c>, are made with the correct input arguments.
    /// </para>
    /// 
    /// <para>
    /// In Pulumi, function calls can be mocked and captured using 
    /// <see cref="IMocks.CallAsync"/>. This test confirms that the function is invoked exactly twice, 
    /// and that each invocation includes the expected <c>roleDefinitionId</c> input.
    /// </para>
    /// </summary>
    [Fact]
    public async Task Call_Input()
    {
        var mocks = new Mocks();
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
        var mocks = new Mocks();
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
        var mocks = new Mocks();
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
