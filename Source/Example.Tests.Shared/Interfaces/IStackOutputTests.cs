namespace Example.Tests.Shared.Interfaces;

/// <summary>
/// <para>
/// Defines a contract for testing Pulumi stack output values.
/// </para>
///
/// <para>
/// In Pulumi's standard testing model, stack outputs are exposed via a property named <c>Outputs</c>,
/// which contains a dictionary of key-value pairs where values may be wrapped in <c>Output&lt;T&gt;</c>.
/// Extracting values from this dictionary requires repetitive code to cast, unwrap, and await these outputs,
/// which introduces noise and reduces test readability.
/// </para>
///
/// <para>
/// <c>Pulumock</c> improves this experience by exposing stack outputs through a clearly named 
/// <c>StackOutputs</c> property and providing extension methods such as <c>RequireValueAsync</c>.
/// These methods abstract away the complexity of accessing and unwrapping output values, allowing
/// tests to express intent more directly.
/// </para>
/// </summary>
public interface IStackOutputTests
{
    #pragma warning disable CA1054
    /// <summary>
    /// <para>
    /// Verifies that a mocked resource output is correctly propagated through the stack
    /// and exposed as a stack output.
    /// </para>
    ///
    /// <para>
    /// This test injects a mocked <c>VaultUri</c> into a <c>KeyVault</c> resource, then asserts
    /// that the corresponding stack output <c>keyVaultUri</c> reflects the same value.
    /// </para>
    /// </summary>
    Task ShouldBeTestable_StackOutputValue(string mockedVaultUri);
    #pragma warning restore CA1054
}
