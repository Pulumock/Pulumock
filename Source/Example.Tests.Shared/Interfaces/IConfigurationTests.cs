namespace Example.Tests.Shared.Interfaces;

public interface IConfigurationTests
{
    Task ShouldBeTestable_ConfigurationValue();
    Task ShouldBeTestable_SecretConfigurationValue();
    Task ShouldBeTestable_DynamicOverriddenConfigurationValue(string tenantId);
    Task ShouldBeTestable_MissingSingleRequiredConfigurationValue();
    Task ShouldBeTestable_MissingAllRequiredConfigurationValue();
}
