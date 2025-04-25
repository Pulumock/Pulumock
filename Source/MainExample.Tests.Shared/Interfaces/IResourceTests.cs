using Pulumi;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.Resources;
using Pulumi.Testing;

namespace MainExample.Tests.Shared.Interfaces;

public interface IResourceTests
{
    /// <summary>
    /// Verifies that an Input-only property (<see cref="Pulumi.AzureNative.Resources.ResourceGroupArgs.ResourceGroupName"/>) is correctly passed to the resource.
    /// Since Input-only values are not returned from <see cref="CustomResource"/>, they must be tracked in the <see cref="IMocks"/> implementation.
    /// </summary>
    Task Resource_InputOnly();
    
    /// <summary>
    /// Validates an Output-only property (<see cref="Pulumi.AzureNative.Resources.ResourceGroup.AzureApiVersion"/>).
    /// Since it has no input, its value must be mocked and asserted from the resource's Output.
    /// </summary>
    Task Resource_OutputOnly();
    
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
    Task Resource_InputOutput();
    
    /// <summary>
    /// Tests a dependency where one resource's Input (<see cref="VaultArgs.ResourceGroupName"/>) 
    /// depends on another resource's Output (<see cref="ResourceGroup.Name"/>), ensuring correct value propagation.
    /// </summary>
    Task Resource_Dependency();
    
    /// <summary>
    /// Asserting on multiple resources
    /// </summary>
    Task Resource_Multiple();
}
