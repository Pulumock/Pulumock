using Pulumi;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.Resources;
using Pulumi.Testing;

namespace Example.Tests.Shared.Interfaces;

/// <summary>
/// <para>
/// Defines a contract for testing Pulumi resource behavior, including input validation, output mocking, 
/// and correct dependency wiring.
/// </para>
///
/// <para>
/// In Pulumi’s default testing framework, mocking resources requires implementing <see cref="IMocks"/> 
/// and overriding <c>NewResourceAsync</c>. To mock a resource, you must match its type using its fully 
/// qualified schema token (e.g., <c>azure-native:keyvault:Vault</c>), which often requires inspecting 
/// source code or documentation. You then construct output dictionaries manually and carefully merge them 
/// with inputs to reflect Pulumi's behavior where some inputs are mirrored in outputs. If you want to target 
/// specific resources by logical name, you also need additional conditionals to match <c>args.Name</c>.
/// </para>
///
/// <para>
/// <c>Pulumock</c> simplifies this process with fluent APIs such as <c>WithMockResource</c> and 
/// <c>WithoutMockResource</c>, allowing developers to add or remove mocks for specific resource types or 
/// even specific named instances. Instead of dealing with stringly-typed dictionaries, you can use 
/// <c>MockResourceBuilder</c> and chain <c>WithOutput</c> calls using strongly-typed expressions.
/// </para>
///
/// <para>
/// Additionally, Pulumock enhances test observability by exposing <c>EnrichedResource</c> objects, 
/// which include not just resolved outputs, but also the raw inputs passed to the resources.
/// </para>
/// </summary>
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
