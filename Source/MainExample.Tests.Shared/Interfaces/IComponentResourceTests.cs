namespace MainExample.Tests.Shared.Interfaces;

public interface IComponentResourceTests
{
    Task ComponentResource();
    Task ComponentResource_MissingNonRequiredResourceArg();
    Task ComponentResource_Parent();
    Task ComponentResource_Outputs();
}
