using System.Collections.Immutable;

namespace Pulumock.Utilities;

/// <summary>
/// Provides logic for merging outputs, including deep merging of nested structures.
/// Used to safely combine Pulumi inputs and outputs where property values may overlap or nest.
/// </summary>
internal static class OutputMerger
{
    /// <summary>
    /// Merges two dictionaries, with the second overriding values in the first.
    /// Nested dictionaries are merged recursively.
    /// </summary>
    /// <param name="baseDict">The base dictionary to merge into.</param>
    /// <param name="overrideDict">The dictionary whose values take precedence.</param>
    /// <returns>A new immutable dictionary containing the merged result.</returns>
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

    /// <summary>
    /// Converts a dictionary to a mutable <see cref="Dictionary{TKey, TValue}"/>, recursively cloning nested dictionaries.
    /// </summary>
    /// <param name="dict">The dictionary to clone.</param>
    /// <returns>A mutable clone of the input dictionary.</returns>
    private static Dictionary<string, object> ToMutable(IReadOnlyDictionary<string, object> dict) =>
        dict.ToDictionary(
            kvp => kvp.Key,
            kvp => TryAsDictionary(kvp.Value, out Dictionary<string, object> nested)
                ? ToMutable(nested)
                : kvp.Value
        );

    /// <summary>
    /// Attempts to treat the given object as a dictionary of <c>string</c> to <c>object</c>.
    /// Supports both mutable and immutable dictionary types.
    /// </summary>
    /// <param name="obj">The object to test and convert.</param>
    /// <param name="dict">The resulting dictionary if conversion is successful.</param>
    /// <returns><c>true</c> if the object was a supported dictionary type; otherwise, <c>false</c>.</returns>
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
