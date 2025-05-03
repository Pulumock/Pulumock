using Pulumi.Testing;

namespace Example.Tests.Shared.Interfaces;

/// <summary>
/// <para>
/// Defines a suite of tests for verifying behavior of
/// <see href="https://www.pulumi.com/docs/iac/concepts/resources/functions/">Pulumi provider function calls</see>,
/// including how they are invoked, what inputs they receive, and what outputs they return.
/// </para>
///
/// <para>
/// In Pulumi's default testing model, provider function calls must be manually mocked by implementing
/// the <see cref="IMocks"/> interface and overriding the <c>CallAsync</c> method. To mock a specific
/// function (e.g., <c>azure-native:authorization:getRoleDefinition</c>), you must look up or extract the 
/// fully qualified type token from Pulumi's resource schema, intercept the call via conditional logic,
/// and construct input/output dictionaries using magic strings that correspond to the function's fields.
/// </para>
///
/// <para>
/// <c>Pulumock</c> replaces this manual setup with a fluent, strongly-typed API. You can use 
/// <c>WithMockCall</c> and <c>WithoutMockCall</c> to dynamically inject or remove mocked function calls
/// as part of your test builder chain. To mock a function, you use <c>MockCallBuilder</c>, which lets
/// you call <c>WithOutput</c> methods using lambda expressions to set output values programmatically,
/// then finalize the mock with <c>Build(typeof(SomeFunctionCall))</c>. This eliminates the need for
/// schema lookups or hardcoded field names.
/// </para>
///
/// <para>
/// Additionally, <c>Pulumock</c> exposes all captured provider function calls post-deployment via the
/// <c>EnrichedStackCalls</c> collection. This allows to inspect which functions were called,
/// with what inputs, and what outputs they produced. This enables assertions such as verifying 
/// invocation count, validating inputs and mocked outputs.
/// </para>
/// </summary>
public interface ICallTests
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
    Task Call_Input();
    
    /// <summary>
    /// <para>
    /// Verifies that all <c>azure-native:authorization:getRoleDefinition</c> function calls made by the stack
    /// that match a specific <c>roleDefinitionId</c> input also return the expected <c>Id</c> output value.
    /// </para>
    ///
    /// <para>
    /// The test sets up a mock response for <c>getRoleDefinition</c> with a known <c>Id</c> output. It then filters
    /// all captured calls to this function by their <c>roleDefinitionId</c> input, extracts the <c>Id</c> output
    /// from each matching call, and asserts that each one equals the mocked value.
    /// </para>
    /// 
    /// <para>
    /// This ensures that mocked function outputs are correctly attached to the appropriate invocations
    /// and can be validated after stack execution.
    /// </para>
    /// </summary>
    Task Call_Output();
    
    /// <summary>
    /// <para>
    /// Verifies that a <c>RoleAssignment</c> resource correctly depends on the result of a 
    /// <c>azure-native:authorization:getRoleDefinition</c> function call by using its output value.
    /// </para>
    ///
    /// <para>
    /// The test mocks the <c>getRoleDefinition</c> call to return a known <c>Id</c> output. After the stack
    /// is deployed, it retrieves the <c>RoleAssignment</c> resource and asserts that its 
    /// <c>RoleDefinitionId</c> property resolves to the expected mocked value.
    /// </para>
    /// 
    /// <para>
    /// This ensures that the resource-to-function-call wiring is preserved and that Pulumi correctly propagates
    /// the output of a function call into a dependent resource property.
    /// </para>
    /// </summary>
    Task Call_ResourceDependency();
}
