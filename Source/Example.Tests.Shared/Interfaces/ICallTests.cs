using Pulumi.Testing;

namespace Example.Tests.Shared.Interfaces;

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
    
    Task Call_Output();
    
    Task Call_ResourceDependency();
}
