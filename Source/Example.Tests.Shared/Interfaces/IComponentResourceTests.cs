using Pulumi.Testing;

namespace Example.Tests.Shared.Interfaces;

/// <summary>
/// <para>
/// Defines a contract for testing Pulumi <see cref="Pulumi.ComponentResource"/> behavior.
/// </para>
///
/// <para>
/// In Pulumi’s standard testing framework, mocking component resources follows the same pattern as for 
/// other resources and function calls: by implementing <see cref="IMocks"/>.
/// While testing basic component construction and inputs is straightforward, 
/// a key limitation is the lack of built-in support for asserting parent-child relationships between resources.
/// </para>
///
/// <para>
/// <c>Pulumock</c> maintains the same fluent mocking approach used for regular resources and calls. 
/// In addition, it enables parent-child assertions by exposing <c>IsChildOf</c> and <c>HasChildren</c> 
/// methods, allowing tests to verify whether resources are correctly nested within component resources. 
/// This makes it possible to validate hierarchical relationships and resource grouping.
/// </para>
/// </summary>
public interface IComponentResourceTests
{
    Task ComponentResource();
    Task ComponentResource_MissingNonRequiredResourceArg();
    Task ComponentResource_Parent();
    Task ComponentResource_Outputs();
}
