using System.Collections.Immutable;

namespace Pulumock.Utilities;

internal sealed class OutputMerger
{
    public static ImmutableDictionary<string, object> Merge(
        IReadOnlyDictionary<string, object> baseDict,
        IReadOnlyDictionary<string, object> overrideDict)
    {
        Dictionary<string, object> result = ToMutable(baseDict);

        foreach ((string key, object overrideValue) in overrideDict)
        {
            if (result.TryGetValue(key, out object? baseValue) &&
                TryAsDictionary(baseValue, out Dictionary<string, object> baseNested) &&
                TryAsDictionary(overrideValue, out Dictionary<string, object> overrideNested))
            {
                result[key] = Merge(baseNested, overrideNested);
            }
            else
            {
                result[key] = overrideValue;
            }
        }

        return result.ToImmutableDictionary();
    }

    private static Dictionary<string, object> ToMutable(IReadOnlyDictionary<string, object> dict) =>
        dict.ToDictionary(
            kvp => kvp.Key,
            kvp => TryAsDictionary(kvp.Value, out Dictionary<string, object> nested)
                ? ToMutable(nested)
                : kvp.Value
        );

    private static bool TryAsDictionary(object? obj, out Dictionary<string, object> dict)
    {
        switch (obj)
        {
            case ImmutableDictionary<string, object> immutable:
                dict = immutable.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                return true;

            case Dictionary<string, object> d:
                dict = d;
                return true;

            default:
                dict = null!;
                return false;
        }
    }
}
