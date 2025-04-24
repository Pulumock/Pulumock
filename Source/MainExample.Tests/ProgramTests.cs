using System.Collections.Immutable;
using System.Text.Json;
using MainExample.Stacks;
using Pulumi;
using Pulumi.AzureNative.Authorization;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.KeyVault.Outputs;
using Pulumi.AzureNative.Resources;
using Pulumi.Testing;
using Pulumi.Utilities;
using Shouldly;
using Deployment = Pulumi.Deployment;
using Resource = Pulumi.Resource;

namespace MainExample.Tests;

// TODO: test component resource
// - Required args
// - Parent
public class ProgramTests
{
    private const string StackName = "dev";
    
    public ProgramTests() => 
        Environment.SetEnvironmentVariable("PULUMI_CONFIG", JsonSerializer.Serialize(new Dictionary<string, object>
        {
            { "azure-native:tenantId", "1f526cdb-1975-4248-ab0f-57813df294cb" },
            { "azure-native:subscriptionId", "f2f2c6e5-17c2-4dfa-913d-6509deb6becf" },
            { "azure-native:location", "swedencentral" },
            { "project:useKeyVaultWithSecretsComponentResource", "true" },
            { "project:databaseConnectionString", "very-secret-value" }
        }));
    
    // Config and secret
    [Fact]
    public async Task Config_ShouldUseMockedConfigurationInResource()
    {
        (ImmutableArray<Resource> Resources, IDictionary<string, object?> StackOutputs) result = await Deployment.TestAsync(
            new Mocks(), 
            new TestOptions {IsPreview = false},
            async () => await CoreStack.DefineResourcesAsync(StackName));

        Vault keyVault = result.Resources
            .OfType<Vault>()
            .Single(x => x.GetResourceName().Equals("microservice-kv-vault", StringComparison.Ordinal));
        
        VaultPropertiesResponse keyVaultProperties = await OutputUtilities.GetValueAsync(keyVault.Properties);
        keyVaultProperties.TenantId.ShouldBe("1f526cdb-1975-4248-ab0f-57813df294cb");
    }
    
    [Fact]
    public async Task Config_ShouldUseMockedConfigurationSecretInResource()
    {
        (ImmutableArray<Resource> Resources, IDictionary<string, object?> StackOutputs) result = await Deployment.TestAsync(
            new Mocks(), 
            new TestOptions {IsPreview = false},
            async () => await CoreStack.DefineResourcesAsync(StackName));

        Secret secret = result.Resources
            .OfType<Secret>()
            .Single(x => x.GetResourceName().Equals("microservice-kv-secret-Database--ConnectionString", StringComparison.Ordinal));
        
        SecretPropertiesResponse secretProperties = await OutputUtilities.GetValueAsync(secret.Properties);
        secretProperties.Value.ShouldBe("very-secret-value");
    }
    
    
    
    // Stack reference
    [Fact]
    public async Task StackReference_ShouldUseMockedStackReferenceInResource()
    {
        (ImmutableArray<Resource> Resources, IDictionary<string, object?> StackOutputs) result = await Deployment.TestAsync(
            new Mocks(), 
            new TestOptions {IsPreview = false},
            async () => await CoreStack.DefineResourcesAsync(StackName));

        RoleAssignment roleAssignment = result.Resources
            .OfType<RoleAssignment>()
            .Single(x => x.GetResourceName().Equals("microservice-ra-kvReader", StringComparison.Ordinal));
        
        string principalId = await OutputUtilities.GetValueAsync(roleAssignment.PrincipalId);
        principalId.ShouldBe("b95a4aa0-167a-4bc2-baf4-d43a776da1bd");
    }
    
    
    
    // Resources
    
    /// <summary>
    /// Verifies that an Input-only property (<see cref="ResourceGroupArgs.ResourceGroupName"/>) is correctly passed to the resource.
    /// Since Input-only values are not returned from <see cref="CustomResource"/>, they must be tracked in the <see cref="IMocks"/> implementation.
    /// </summary>
    [Fact]
    public async Task Resource_InputOnly()
    {
        var mocks = new Mocks();
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
    /// Validates an Output-only property (<see cref="ResourceGroup.AzureApiVersion"/>).
    /// Since it has no input, its value must be mocked and asserted from the resource's Output.
    /// </summary>
    [Fact]
    public async Task Resource_OutputOnly()
    {
        (ImmutableArray<Resource> Resources, IDictionary<string, object?> StackOutputs) result = await Deployment.TestAsync(
            new Mocks(), 
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
        var mocks = new Mocks();
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
        var mocks = new Mocks();
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
    
    
    
    // Calls
//   - Resource dependencies on call
    
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
    
    
    
    // Dynamic values
    [Theory]
    [InlineData("dev")]
    [InlineData("prod")]
    public async Task Stack_ShouldDynamicallyApplyStackName(string stackName)
    {
        var mocks = new Mocks();
        _ = await Deployment.TestAsync(
            mocks, 
            new TestOptions {IsPreview = false},
            async () => await CoreStack.DefineResourcesAsync(stackName));
        
        ResourceSnapshot resourceSnapshot = mocks.ResourceSnapshots.Single(x => x.LogicalName.Equals("microservice-kv-vault", StringComparison.Ordinal));
        if (!resourceSnapshot.Inputs.TryGetValue("vaultName", out object? value) || value is not string vaultName)
        {
            throw new KeyNotFoundException("Input with key 'vaultName' was not found or is not of type string.");
        }
        
        vaultName.ShouldBe($"microservice-kv-{stackName}");
    }
    // TODO: if-statement on component resource
    
    
    // Stack outputs
    [Fact]
    public async Task StackOutputs_ShouldOutputMockedValue()
    {
        (ImmutableArray<Resource> Resources, IDictionary<string, object?> StackOutputs) result = await Deployment.TestAsync(
            new Mocks(), 
            new TestOptions {IsPreview = false},
            async () => await CoreStack.DefineResourcesAsync(StackName));
        
        Vault keyVault = result.Resources
            .OfType<Vault>()
            .Single(x => x.GetResourceName().Equals("microservice-kv-vault", StringComparison.Ordinal));
        
        VaultPropertiesResponse keyVaultProperties = await OutputUtilities.GetValueAsync(keyVault.Properties);
        
        string keyVaultUriStackOutput = result.StackOutputs["keyVaultUri"] is Output<string> vaultUriOutput
            ? await OutputUtilities.GetValueAsync(vaultUriOutput)
            : throw new InvalidOperationException("keyVaultUri was not an Output<string>");

        keyVaultUriStackOutput.ShouldBe(keyVaultProperties.VaultUri);
    }
}
