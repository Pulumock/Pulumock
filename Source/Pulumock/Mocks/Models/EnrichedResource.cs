using System.Collections.Immutable;

namespace Pulumock.Mocks.Models;

/// <summary>
/// Represents an intercepted Pulumi resource enriched with its type token, logical name, and raw input values.
/// </summary>
/// <param name="TypeToken">The fully qualified resource type token (e.g., <c>"azure-native:keyvault:Vault"</c>).</param>
/// <param name="LogicalName">The logical name of the resource as defined in the Pulumi program.</param>
/// <param name="Inputs">The raw inputs passed to the resource as defined in the Pulumi program.</param>
public sealed record EnrichedResource(string? TypeToken, string LogicalName, ImmutableDictionary<string, object> Inputs);
