using Pulumi;

namespace Example.Tests.Shared.Interfaces;

/// <summary>
/// <para>
/// Provides a contract for tests that validate Pulumi stack configuration handling.
/// </para>
///
/// <para>
/// Pulumi programs that rely on stack configuration (e.g., <c>new Config().Require("azure-native")</c>) must 
/// ensure these values are mocked during test execution. By default, Pulumi retrieves configuration 
/// from the <c>PULUMI_CONFIG</c> environment variable, which must be set manually using a JSON structure:
/// <c>{ "project:myvar": "myvalue" }</c>.
/// </para>
///
/// <para>
/// The <c>Pulumock</c> framework addresses this limitation by offering a fluent, programmatic interface.
/// Using methods like <c>WithMockStackConfiguration</c>, <c>WithoutMockStackConfiguration</c>, and <c>ClearMockStackConfigurations</c>,
/// configuration values can be injected, removed, or reset dynamically through method chaining.
/// Under the hood, Pulumock still relies on the <c>PULUMI_CONFIG</c> environment variable to simulate Pulumi’s behavior, but it abstracts
/// away the manual JSON construction and environment setup required by the default approach.
/// </para>
///
/// <para>
/// Note: Since <c>PULUMI_CONFIG</c> is a global environment variable, tests that use 
/// mocked configuration cannot safely run in parallel. This aligns with Pulumi’s own constraints, as its 
/// <c>TestAsync</c> methods are not parallel-safe due to shared global state.
/// </para>
/// </summary>
public interface IConfigurationTests
{
    /// <summary>
    /// <para>
    /// Verifies that a configuration value from the Pulumi stack configuration is correctly passed into a resource input.
    /// </para>
    ///
    /// <para>
    /// This test mocks the <c>tenantId</c> configuration value under the <c>azure-native</c> namespace.
    /// After the stack is deployed, it retrieves the <c>KeyVault</c> resource and asserts that its
    /// <c>TenantId</c> input property reflects the configured value.
    /// </para>
    /// </summary>
    Task ShouldBeTestable_ConfigurationValue();
    
    /// <summary>
    /// <para>
    /// Verifies that a secret value from the Pulumi stack configuration is correctly passed into a resource input.
    /// </para>
    ///
    /// <para>
    /// This test mocks the <c>databaseConnectionString</c> secret configuration value and deploys the stack.
    /// It then retrieves the corresponding <c>Secret</c> resource and asserts that the <c>Value</c>
    /// property in its resolved output matches the expected secret.
    /// </para>
    /// </summary>
    Task ShouldBeTestable_SecretConfigurationValue();
    
    /// <summary>
    /// <para>
    /// Verifies that dynamically provided Pulumi configuration values are correctly passed
    /// into resource inputs, where the last one wins.
    /// </para>
    ///
    /// <para>
    /// This parameterized test overrides the <c>azure-native:tenantId</c> configuration key
    /// with different values. After deploying the stack, it asserts that the
    /// <c>TenantId</c> input on the <c>KeyVault</c> resource reflects the overridden value in each case.
    /// </para>
    /// </summary>
    Task ShouldBeTestable_DynamicOverriddenConfigurationValue(string tenantId);
    
    /// <summary>
    /// <para>
    /// Verifies that the stack fails to deploy when a single required configuration value is missing.
    /// </para>
    ///
    /// <para>
    /// This test explicitly removes the <c>azure-native:tenantId</c> configuration key before deployment.
    /// It asserts that a <see cref="RunException"/> is thrown, indicating that the stack correctly enforces
    /// required configuration presence.
    /// </para>
    /// </summary>
    Task ShouldBeTestable_MissingSingleRequiredConfigurationValue();
    
    /// <summary>
    /// <para>
    /// Verifies that the stack fails to deploy when all required configuration values are missing.
    /// </para>
    ///
    /// <para>
    /// This test clears the entire mock configuration set before deployment.
    /// It asserts that a <see cref="RunException"/> is thrown, confirming that the stack cannot initialize
    /// without the required configuration inputs.
    /// </para>
    /// </summary>
    Task ShouldBeTestable_MissingAllRequiredConfigurationValue();
}
