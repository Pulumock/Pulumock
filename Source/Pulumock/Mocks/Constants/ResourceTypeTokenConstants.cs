namespace Pulumock.Mocks.Constants;

/// <summary>
/// Defines constants for Pulumi resource type tokens.
/// <para/>
/// A <see href="https://www.pulumi.com/docs/iac/adopting-pulumi/import/#where-to-find">type token</see>
/// is a unique string identifier used by Pulumi to identify a specific resource type.
/// </summary>
internal static class ResourceTypeTokenConstants
{
    /// <summary>
    /// The type token for <see cref="Pulumi.StackReference"/>.
    /// </summary>
    public const string StackReference = "pulumi:pulumi:StackReference";
}
