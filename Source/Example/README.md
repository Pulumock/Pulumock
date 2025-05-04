# Example

The `Example` project contains a complete, working Pulumi program that can be tested and deployed. 
It defines a full infrastructure stack and depends on the `StackReference` project to test stack reference scenarios.

The example demonstrates how to use the Pulumock library by comparing test implementations with and without it:

- [Example.Tests.Shared](../Example.Tests.Shared): Defines shared interfaces and provides detailed explanations for all test scenarios.
- [Example.Tests.WithoutPulumock](../Example.Tests.WithoutPulumock): Implements the shared interfaces using standard Pulumi.NET testing tools, without Pulumock.
- [Example.Tests.WithPulumock](../Example.Tests.WithPulumock): Implements the shared interfaces using Pulumock for mocking and testing.

