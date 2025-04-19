using System.Collections.Immutable;

namespace Pulumock.Mocks.Models;

/// <summary>
/// Represents a mocked Pulumi configuration.
/// </summary>
/// <param name="MockConfigurations">
/// A dictionary of configuration key-value pairs to simulate Pulumi config values in a test environment.
/// Keys follow the Pulumi configuration format (e.g., "project:key", "provider:key").
/// Values can be plain types, structured objects, or secret-wrapped values.
/// </param>
public record MockConfiguration(ImmutableDictionary<string, object> MockConfigurations);
