namespace AoCDay7Puzzle1;

internal static class OperatorHelper
{
    private static readonly Dictionary<int, IReadOnlyList<string>> OperatorsByLengthCache = new()
    {
        [0] = [],
        [1] = ["+", "*"],
    };

    public static IReadOnlyList<string> CreateAllPossibleOperatorCombinations(int length)
    {
        if (OperatorsByLengthCache.TryGetValue(length, out var cached))
        {
            return cached;
        }

        var result = new List<string>();
        var temp = CreateAllPossibleOperatorCombinations(length - 1);
        foreach (var val in temp)
        {
            result.Add($"{val}+");
            result.Add($"{val}*");
        }

        OperatorsByLengthCache.Add(length, result);

        return result;
    }
}