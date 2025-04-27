using System.Text.Json;

namespace Example.Tests.WithoutPulumock.Shared;

#pragma warning disable CA1515
public class TestBase
#pragma warning restore CA1515
{
    protected const string StackName = "dev";

    protected TestBase() =>
        Environment.SetEnvironmentVariable("PULUMI_CONFIG", JsonSerializer.Serialize(new Dictionary<string, object>
        {
            { "azure-native:tenantId", "1f526cdb-1975-4248-ab0f-57813df294cb" },
            { "azure-native:subscriptionId", "f2f2c6e5-17c2-4dfa-913d-6509deb6becf" },
            { "azure-native:location", "swedencentral" },
            { "project:stackReferenceOrgName", "hoolit" },
            { "project:stackReferenceProjectName", "StackReference" },
            { "project:databaseConnectionString", "very-secret-value" }
        }));
}
