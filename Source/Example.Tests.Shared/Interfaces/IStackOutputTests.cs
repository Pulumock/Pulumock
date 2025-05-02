namespace Example.Tests.Shared.Interfaces;

public interface IStackOutputTests
{
    #pragma warning disable CA1054
    Task ShouldBeTestable_StackOutputValue(string mockedVaultUri);
    #pragma warning restore CA1054
}
