using Pulumi;
using Pulumi.Testing;

namespace Example.Tests.Shared.Interfaces;

/// <summary>
/// <para>
/// Defines a contract for testing behavior that involves <see cref="StackReference"/> resources.
/// </para>
///
/// <para>
/// In Pulumi's default testing framework, stack references must be explicitly mocked, or the program
/// will fail with a <see cref="RunException"/>. This is done by implementing <see cref="IMocks"/> and overriding
/// <c>NewResourceAsync</c>. To intercept a stack reference, developers must manually check whether
/// <c>args.Type</c> equals the magic string <c>pulumi:pulumi:StackReference</c>. If the reference is
/// stack-specific, you must also match against the fully qualified name (e.g., <c>org/project/stack</c>),
/// which can be dynamic during test runs. Outputs must then be mocked by manually constructing
/// dictionaries with hardcoded keys and values.
/// </para>
/// <para>
/// <c>Pulumock</c> streamlines this workflow through a fluent, type-safe API. Using methods like 
/// <c>WithMockStackReference</c> and <c>WithoutMockStackReference</c>, developers can programmatically 
/// add or remove mocked stack references within the test builder. The <c>MockStackReferenceBuilder</c>
/// allows for fluent chaining of <c>WithOutput</c> calls to specify expected output values, and is finalized 
/// via <c>Build()</c>. This removes the need for magic strings, type matching, and manual dictionary creation.
/// </para>
/// </summary>
public interface IStackReferenceTests
{
    /// <summary>
    /// Verifies that a stack reference with a dynamic fully qualified name correctly resolves and returns 
    /// the expected output value used by dependent resources.
    /// </summary>
    Task ShouldBeTestable_DynamicFullyQualifiedStackName(string stackName);
    
    /// <summary>
    /// Verifies that a stack reference returns different valid output values and that those values are 
    /// correctly assigned to resource properties.
    /// </summary>
    Task ShouldBeTestable_DynamicValidOutputValues(string stackReferenceOutputValue);
    
    /// <summary>
    /// Verifies that when a required stack reference is missing, the deployment fails with a <see cref="RunException"/>.
    /// </summary>
    Task ShouldBeTestable_MissingOutputValue();
    
    /// <summary>
    /// Verifies that a stack reference fails as expected when the output value has an invalid type.
    /// </summary>
    Task ShouldBeTestable_InvalidOutputValueType();
}
