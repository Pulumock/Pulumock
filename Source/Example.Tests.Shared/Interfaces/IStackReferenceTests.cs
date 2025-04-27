namespace Example.Tests.Shared.Interfaces;

public interface IStackReferenceTests
{
    Task ShouldBeTestable_DynamicFullyQualifiedStackName(string stackName);
    Task ShouldBeTestable_DynamicValidOutputValues(string stackReferenceOutputValue);
    Task ShouldBeTestable_MissingOutputValue();
    Task ShouldBeTestable_InvalidOutputValueType();
}
