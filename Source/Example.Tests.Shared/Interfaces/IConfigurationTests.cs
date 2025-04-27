namespace Example.Tests.Shared.Interfaces;

public interface IConfigurationTests
{
    Task Config_MockedConfigurationInResource();
    Task Config_MockedSecretInResource();
}
